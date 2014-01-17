Zest
====================
A C# test generator for the MSpec test framework(https://github.com/machine/machine.specifications). 

This project will define a DSL, called zest, that can be used to generate mspec tests.

The primary aim of this project is to reduce the amount of boilerplate code one has to produce when writing tests.

Zest Compiler
-
The zest compiler is written in C# and uses irony to parse the zest grammar.

To build a NuGet package for the compiler and to run its tests:
--
open powershell
cd into the compiler directory
```
Import-Module .\tools\psake.psm1
Import-Module .\tools\igi.psm1
Invoke-psake
```
