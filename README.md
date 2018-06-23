# dotnet-ildiff
[![Build status](https://ci.appveyor.com/api/projects/status/290wu3l8a7ja5uxw?svg=true)](https://ci.appveyor.com/project/pjbgf/dotnet-ildiff)
[![Nuget](https://img.shields.io/nuget/dt/dotnet-ildiff.svg)](http://nuget.org/packages/dotnet-ildiff) 
[![Nuget](https://img.shields.io/nuget/v/dotnet-ildiff.svg)](http://nuget.org/packages/dotnet-ildiff) 
[![License](http://img.shields.io/:license-mit-blue.svg)](http://pjbgf.mit-license.org)  

# Description
The `dotnet ildiff` compares the IL difference between two .NET assemblies. Simply send the assemblies path as parameter and as a result you will get the diff.

# Setup
The project was created as a global CLI tool, therefore you can install with a single command:  

`dotnet tool install -g dotnet-diff`

Notice that for the command above to work, you need .NET Core SDK 2.1.300 or above installed in your machine.

# Syntax
```
dotnet ildiff <ASSEMBLY1_PATH> <ASSEMBLY2_PATH>
dotnet ildiff <ASSEMBLY1_PATH> <ASSEMBLY2_PATH> <-o|--output>
dotnet ildiff <ASSEMBLY1_PATH> <ASSEMBLY2_PATH> <-i|--item>
dotnet ildiff <-h|--help>
```

# Options
`-i`  
Filter results by method and/or classes to be disassembled.

`-o`  
Define the output file to be created with the assembly's IL.

# Examples
Output IL diff to the command line:
```
dotnet ildiff myassembly1.dll myassembly2.dll
```

Filter results by method and/or classes to be disassembled, showing the result in the command line:
```
dotnet ildiff myassembly1.dll myassembly2.dll -i ClassName
dotnet ildiff myassembly1.dll myassembly2.dll -i ::MethodName
dotnet ildiff myassembly1.dll myassembly2.dll -i ClassName::MethodName
```

Define the file to be created with the output: 
```
dotnet ildiff myassembly1.dll myassembly2.dll -o disassembledAssembly.il
```
  
# Powered by
This tool was developed and is maintained with JetBrains Rider: the cross-platform and lightweight .NET/C# IDE which comes with ReSharper integrated. For more information check [JetBrains' website](https://www.jetbrains.com/rider).
