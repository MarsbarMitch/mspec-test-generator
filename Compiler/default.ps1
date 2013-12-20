#Author: Anthony Jones
# example1.ps1: A project containing code, unit and integrations tests, and exporting one or more packages.

# In most cases these are the only $build variables that you'll have to assign to. However, the other members of the $build hash may
# also be changed if you don't like the default values.
$build.code_projects = @("Compiler")
$build.test_projects = @("Compiler.Tests")
$framework = "4.0"

# NOTE: When invoke-psake runs this build file, it will create packages and place them under Delivery\Packages by default.
# Hudson's configuration should call invoke-psake ExportPackages to ensure that Bob also pushes the packages to the package feed.
task Default -depends ExportPackages

task Setup {
	Clear-Environment
	Initialize-Environment
}

task Build -depends Setup {
	Invoke-Build
}

task Test -depends Build {
	Invoke-Tests
}

task ExportPackages -depends Test {

	# Export-Packages looks for a .nuspec or .nuspect (nuspec template) file in each code project directory (i.e. next to your project files).
	#    - If it finds a .nuspec file then it will use that to generate packages.
	#    - Otherwise, if it finds a .nuspect file and packages.config file then it will complete your template file
	#      with 'hard' versioned dependency references (e.g. version=[1.0]) and use the resulting .nuspec file to generate packages.
	#    - Failing both of the above it will skip the current project (with a warning message).

	Export-Packages
}

task PublishPackages -depends ExportPackages {
	Publish-Packages
}

# NOTE that this file is still a psake build file and PowerShell script file: you can still add variables, tasks and functions of your own.