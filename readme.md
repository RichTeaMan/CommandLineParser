# RichTea.CommandLineParser

![Build](https://github.com/RichTeaMan/CommandLineParser/workflows/Build/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/RichTea.CommandLineParser.svg?style=flat)](https://www.nuget.org/packages/RichTea.CommandLineParser/)

This project provides a command line parsing utility.
[Documentation available at richteaman.github.io/CommandLineParser/.](http://richteaman.github.io/CommandLineParser/)

## Cake Tasks

This project uses [Cake](https://cakebuild.net)!
* cake -target=Clean
* cake -target=Restore-Nuget-Packages
* cake -target=Build
* cake -target=Test
* cake -target=Docs
* cake -target=DocsServe

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
