#Author: Anthony Jones
#- PRIVATE FUNCTIONS --------------------------------------------------

function CreateDirectory($directory)
{
	if(!(Test-Path $directory))
	{
		mkdir $directory | Out-Null
	}
}

function GetTestRunner
{
	$result = $null
	
	$packages_path = "$($build.root_directory)\packages"
	if(Test-Path $packages_path)
	{
		$exe_path = Get-ChildItem -Path "$packages_path\Machine.Specifications.*\tools\mspec-x86-clr4.exe"
		if($exe_path -ne $null)
		{
			$result = $exe_path.FullName
		}
	}
	
	return $result
}

function WriteFeedback([string]$message)
{
	Write-Host $message -ForegroundColor Green
}

function GetVersionNumber()
{
	$version = $env:hudson_build_number
	if($version)
	{
		# pattern defines a version number formatted as: a.b.c.d
		$pattern = '^(\d+\.\d+)\.\d+\.(\d+)$'

		if($version -match $pattern)
		{
			# the LOCAL revision number for the current repository, minus the modifier character if present
			$revision_number = (hg id --num).Replace("+", "")
			# replace the 'c' part of the version number with the local repository revision number;
			# note the sneaky use of periods to allow escaped dollar characters and string interpolation
			$result = ($version -replace $pattern, "`$1.$revision_number.`$2").Replace("+", "")

			if($result -notmatch $pattern)
			{
				throw "GetVersionNumber created a non-compliant version number: $result"
			}
			
			return $result
		}
	}
}

function GetAssemblyVersion()
{
	# The assembly to read the version info from
	param([parameter(Mandatory=$true)][string]$assembly)

	$hudson_version_number = GetVersionNumber
	$assembly_version_number = (Get-Command $assembly).FileVersionInfo.FileVersion
	return Coalesce $hudson_version_number $assembly_version_number
}

# null coalescing function
function Coalesce($item, $backup)
{
	if($item -ne $null) {	return $item } else { return $backup }
}

function AggregateProjectBuildFiles($projects, $aggregate_directory)
{
	foreach($project in $projects)
	{
		$build_directory = "$($build.build_directory)\$project"
		Copy-Item "$build_directory\*.*" $aggregate_directory -Force
	}
}

function GetPackagesFromConfigFile($config_path)
{
	$packages = @()
	
	$file = [xml](Get-Content $config_path)
	($file.packages.ChildNodes | Group-Object Id) | Foreach-Object {
		# this line just makes sure that if we have multiple versions of any packages listed then the latest package
		# will be selected; otherwise we still need to select singly listed packages
		
		$package = $_
		# = (Group.Latest ? Group.Latest : Group) | %{ ...
		# -> should we even allow this to happen?
		(Coalesce (($package.Group | Sort-Object Version -Descending)[0]) $package.Group) | Foreach-Object {
			$new_item = @{}
			$new_item.Id = $_.GetAttribute('id')
			$new_item.Version = "[$($_.GetAttribute('version'))]"
			$packages += $new_item
		}
	}
	
	return $packages
}

function AggregatePackages($projects)
{
	$packages = @()
	
	foreach($project in $projects)
	{
		$config_path = (Get-Item "$project\packages.config")
		if($config_path -eq $null) { throw "Unable to find a packages.config file for project '$project'."  }
	
		$packages += GetPackagesFromConfigFile $config_path
	}
	
	return $packages
}

function WriteTemplate($packages)
{
	$template = [xml](Get-Content $template_path)
	$namespace = $template.package.NamespaceURI

	# locate the element using the string indexers just in case the dependencies element is empty
	# (otherwise we get an empty string back rather than an XmlElement instance)
	$dependencies_element = $template["package"]["metadata"]["dependencies"]
	if($dependencies_element)
	{
		$dependencies_element.RemoveAll()

		$packages | Foreach-Object {
			$new_item = $template.CreateElement("dependency", $namespace)
			$new_item.SetAttribute("id", $_.Id)
			$new_item.SetAttribute("version", $_.Version)

			$dependencies_element.AppendChild($new_item) | Out-Null
		}
	}
	
	$spec_path = $template_path.ToString().Replace(".nuspect",".nuspec")
	$template.Save($spec_path)
}

function CompleteProjectTemplate($projects, $template_path)
{
	if($template_path -eq $null) { throw "Unable to find the given .nuspect template path '$template_path'."	}

	$packages = AggregatePackages $projects
	WriteFeedback "Completing project template(s) for project(s): $($projects -join ', ')"
	
	$grouped_by_id = ($packages | Group-Object { $_.Id })
	$grouped_by_id | Foreach-Object {
		# take a group of packages with the same id and then group again by version; if the result has a Length property
		# then it is a collection and multiple versions of the package are in use
		if(($_.Group | Group-Object {$_.Version}).Length)
		{
			$package_name = $_.Name
			$versions_used = ($_.Group | % {$_.Version}) -join ', '
			throw "Multiple versions of the package '$package_name' are used: $versions_used"
		}
	}

	$packages = ($grouped_by_id | %{ Coalesce (($_.Group | Sort-Object Version -Descending)[0]) $_.Group })
	
	WriteTemplate $packages
}

function CreatePackage([string]$nuspec_path, [string]$assembly_version)
{
	if($assembly_version)
	{
		Exec{ & "$($build.tools_directory)\NuGet.exe" pack "$nuspec_path" -Version $assembly_version -OutputDirectory "$($build.packages_directory)" }
	}
	else
	{
		Exec{ & "$($build.tools_directory)\NuGet.exe" pack "$nuspec_path" -OutputDirectory "$($build.packages_directory)" }
	}
}

function TestConsistentPackageVersions($packages_hash, $all_packages)
{
	($all_packages | Group-Object id) | Foreach-Object {

		$packages_grouped_by_version = ($_.Group | Sort-Object version -Unique)
		if($packages_grouped_by_version.Length -ne $null)# if there's a Length member then Length > 0
		{
			$bad_package_id = $packages_grouped_by_version[0].id
			Write-Host "Different versions of package '$bad_package_id' are used in multiple projects:"

			foreach($key in $packages_hash.Keys)
			{
				$bad_package_reference = $packages_hash[$key] | Where-Object { $_.id -eq $bad_package_id }
				if($bad_package_reference -ne $null)
				{
					Write-Host " - $key uses version $($bad_package_reference.version)."
				}
			}
			ThrowException("Not all projects use the same version of '$bad_package_id'")
		}
	}
}

function TestConsistentPackagesReferences($packages_hash)
{
	# for each known package.config
	foreach($key in $packages_hash.Keys)
	{
		# read the corresponding project file and locate the reference hint paths (these point to the nuget packages used by the project)
		$project_directory = $key.Replace("packages.config", "*.csproj")
		$project = Get-Item $project_directory
		$xml = [xml](Get-Content $project)
		$hint_paths = $xml.Project.ItemGroup | Foreach-Object { $_.Reference | Select-Object HintPath } | Foreach-Object { if($_.HintPath -ne $null) { $_.HintPath.ToString()  } }

		# foreach known package referenced by the current packages.config
		foreach($package in $packages_hash[$key])
		{
			# what should the package folder look like?
			$package_folder_string = "$($package.id).$($package.version)"
			
			# do any of the hint paths contain a matching folder name?
			if(($hint_paths | Where-Object { $_ -Match $package_folder_string }).Length -eq $null)
			{
				Write-Host "For project $key - $package_folder_string is referenced by the packages.config file but not by the project."
				ThrowException("Project references do not match the references in packages.config")
			}
		}
	}
}

function TestAllPackageFoldersUsed($all_packages)
{
	# package folders that are not referenced by any package.config files
	$all_package_folders = $all_packages | sort id -Unique | Foreach-Object { "$($_.id).$($_.version)" }
	(Get-ChildItem .\packages | Where-Object { $_.Attributes -eq "Directory" }) | Where-Object { $all_package_folders -notcontains $_.Name.ToString() } | Foreach-Object { ThrowException("$($_.Name) is not referenced by any of the solution's package.config files.") }
}

function ThrowException($exception_message)
{
	$stack = Get-PSCallStack
	Write-Host "Exception encountered: $exception_message" -foregroundcolor "red"
	#skip the first two stack trace elements as they are related to the exception throwing and are not the cause of the exception
	for($i=2;$i -le ($stack).Count -1; $i++)
	{
		Write-Host " at" $stack[$i].Command  "-"$stack[$i].Location -foregroundcolor "red"
	}
	throw ""
}

#- PUBLIC BUILD HASH---------------------------------------------------

$script:build = @{}
$script:build.root_directory = Get-Location
$script:build.tools_directory = "$($build.root_directory)\tools"
$script:build.delivery_directory = "$($build.root_directory)\Delivery"
$script:build.build_directory = "$($build.delivery_directory)\Build"
$script:build.testing_directory = "$($build.delivery_directory)\Testing"
$script:build.packages_directory = "$($build.delivery_directory)\Packages"
$script:build.solution_name = (Get-ChildItem -Path "$($build.root_directory)\*.sln").Name
$script:build.code_projects = @()# this should be provided by the build script
$script:build.test_projects = @()# this should be provided by the build script
Export-ModuleMember -Variable "build"


#- PUBLIC FUNCTIONS ---------------------------------------------------

function Invoke-StandardBuild
{
	Clear-Environment
	Initialize-Environment
	
	Invoke-Build
	Invoke-Tests
	
	Export-Packages
	Publish-Packages
}

function Clear-Environment
{
	if(Test-Path $build.delivery_directory)
	{
		WriteFeedback "Cleaning delivery directory"
		Remove-Item $build.delivery_directory -Recurse -Force | Out-Null
	}
	
	WriteFeedback "Cleaning solution"
	Exec{ msbuild $build.solution_name /t:Clean /p:Configuration=Release /v:quiet }
}

function Initialize-Environment
{
	CreateDirectory $build.delivery_directory
	CreateDirectory $build.build_directory
	CreateDirectory $build.testing_directory
	CreateDirectory $build.packages_directory
}

function Invoke-Build([array]$projects)
{
	if(!$projects)
	{
		$projects = $build.code_projects + $build.test_projects
	}
	if(!$projects)
	{
		throw "Invoke-Build: Please provide a non-empty projects array or assign your projects to the build.code_projects and build.test_projects variables."
	}

	WriteFeedback "Building projects"
	foreach($project in $projects)
	{
		# msbuild complains about a missing trailing slash unless \\ or / is at the end of $output_directory
		$output_directory = "$($build.build_directory)\$project/"
		CreateDirectory $output_directory
		
		Exec{ msbuild $project\$project.csproj /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$output_directory }
	}
}

function Invoke-Tests([array]$projects)
{
	# test the consitency of the solution first
	Test-Solution
	
	if(!$projects)
	{
		$projects = $build.test_projects
	}
	if(!$projects)
	{
		throw "Invoke-Tests: Please provide a non-empty projects array or assign your test projects to the build.test_projects variable."
	}
	
	$test_runner = GetTestRunner
	if($test_runner -eq $null)
	{
		throw "Can't invoke tests: failed to find test runner."
	}

	WriteFeedback "Running tests"
	
	foreach($project in $projects)
	{
		Exec{ & $test_runner "$($build.build_directory)\$project\$project.dll" --html "$($build.testing_directory)\$project.html" }
	}
}

function Export-Packages($projects)
{
	if(!$projects)
	{
		$projects = $build.code_projects
	}
	if(!$projects)
	{
		throw "Export-Packages: Please provide a non-empty projects array or assign your package projects to the build.code_projects variable."
	}

	WriteFeedback "Creating packages"
	
	# look for a root specification file - if present then create an aggregate package
	$root_nuspec = Get-Item "$($build.root_directory)\*.nuspec"
	$root_nuspect = Get-Item "$($build.root_directory)\*.nuspect"
	if($root_nuspec -or $root_nuspect)
	{
		$root_file = Coalesce $root_nuspec $root_nuspect
		WriteFeedback "Creating aggregate package with specification: '$root_file'."
	
		$aggregate_directory = "$($build.build_directory)\Aggregate"
		CreateDirectory $aggregate_directory
		AggregateProjectBuildFiles $projects $aggregate_directory
		Copy-Item $root_file $aggregate_directory
		
		# complete a nuspect file if present
		if($root_nuspect)
		{
			$aggregate_nuspect = Get-Item "$aggregate_directory\*.nuspect"
			CompleteProjectTemplate $projects $aggregate_nuspect
		}
	
		$aggregate_nuspec = Get-Item "$aggregate_directory\*.nuspec"
		$hudson_version = GetVersionNumber
		CreatePackage $aggregate_nuspec $hudson_version
	}
	else
	{
		foreach($project in $projects)
		{
			$project_directory = "$($build.root_directory)\$project"
			$build_directory = "$($build.build_directory)\$project"
			$assembly_path = "$build_directory\$project.dll"
			$nuspec_path = "$project_directory\$project.nuspec"
			$nuspect_path = "$project_directory\$project.nuspect"
			$packages_config_path = "$project_directory\packages.config"
		
			$assembly_version = GetAssemblyVersion $assembly_path

			# prefer .nuspec files if present
			if(Test-Path $nuspec_path)
			{
				WriteFeedback "- Using .nuspec file for project '$project'."
				
				Copy-Item $nuspec_path $build_directory
				CreatePackage "$build_directory\$project.nuspec" $assembly_version
			}
			# alternatively complete a .nuspect template (requires packages.config to be present)
			elseif((Test-Path $nuspect_path) -and (Test-Path $packages_config_path))
			{
				WriteFeedback "- Using .nuspec template file for project '$project'."
				
				Copy-Item $nuspect_path $build_directory
				Copy-Item $packages_config_path $build_directory
				CompleteProjectTemplate $build_directory (Get-Item "$build_directory\*.nuspect")
	
				CreatePackage "$build_directory\$project.nuspec" $assembly_version
			}
			# otherwise skip this project
			else
			{
				WriteFeedback "- Unable to find package specification for project '$project'."
			}
		}
	}
}

function Publish-Packages([array]$packages)
{
	if(!$packages)
	{
		$packages = Get-ChildItem -Path "$($build.packages_directory)\*.nupkg"
	}

	# getting the build pc ip address:
	# 1. use a DNS lookup to get all address information for the build pc
	# 2. filter these to eliminate any localhost MAC addresses
	# 3. grab the first remaining address and ask for the ip address as a string
	$build_pc_ip = @([System.Net.Dns]::GetHostAddresses("buildpc") | Where-Object{ $_.IsIPv6LinkLocal -eq $false})[0].IpAddressToString
	$push_address = "http://" + $build_pc_ip+ ":8701/"
	
	WriteFeedback "Pushing packages"
	$packages | ForEach-Object {
		Exec{ & "$($build.tools_directory)\NuGet.exe" push $_ -Source $push_address push_package }
	}
}

function Test-Solution()
{
	#build a hash of all packages: packages_config_filename -> packages_referenced
	$packages_hash = @{}
	$pwd_length = (pwd).Path.Length + 1
	foreach($file in (Get-ChildItem -Recurse -Include packages.config))
	{
		$xml = [xml](Get-Content $file)
		$fileName = $file.FullName.ToString().SubString($pwd_length)
		$packages_hash.Add($fileName, $xml.packages.package)
	}
	
	$all_packages = ($packages_hash.Keys | Foreach-Object { $packages_hash[$_] })

	TestConsistentPackageVersions $packages_hash $all_packages
	TestConsistentPackagesReferences $packages_hash
	TestAllPackageFoldersUsed $all_packages
}

Export-ModuleMember -Function Clear-Environment, Initialize-Environment, Invoke-Build, Invoke-Tests, Export-Packages, Publish-Packages, Perform-Build, Test-Solution