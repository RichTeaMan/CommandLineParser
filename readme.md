# RichTea.CommandLineParser

[![NuGet](https://img.shields.io/nuget/v/RichTea.CommandLineParser.svg?style=flat)](https://www.nuget.org/packages/RichTea.CommandLineParser/)

This project provides a command line parsing utility.

## Cake Tasks

This project uses [Cake](https://cakebuild.net)!
* cake -target=Clean
* cake -target=Restore-Nuget-Packages
* cake -target=Build
* cake -target=Test
* cake -target=Docs
* cake -target=DocsServe

## CI

|        | Windows | Linux |
| ------ | --------|-------|
| Master | [![Build status](https://ci.appveyor.com/api/projects/status/gg7er3fta9sjxy5m/branch/master?svg=true)](https://ci.appveyor.com/project/RichTeaMan/commandlineparser/branch/master) | [![Build Status](https://travis-ci.org/RichTeaMan/CommandLineParser.svg?branch=master)](https://travis-ci.org/RichTeaMan/CommandLineParser) |

## Example

```C#

static void Main(string[] args)
{
    Parser.ParseCommand<Program>(args);
}

// methods must be public static
[DefaultClCommand]
public static void ProgramState([ClArgs("resultDirectory", "rd")]string resultDirectory = "Results")
{
    // program code here...
}

```
