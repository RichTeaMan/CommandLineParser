#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin "Cake.Incubator"
#addin "Cake.DocFx"
#tool "docfx.console"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/bin/**");
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreRestore("./CommandLineParser.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    DotNetCoreBuild("./CommandLineParser.sln", new DotNetCoreBuildSettings {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Configuration = configuration
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("CommandLineParser.Tests/CommandLineParser.Tests.csproj");
});

Task("Docs")
	.Does(() =>
{
    DocFxMetadata("./Docfx/docfx.json");
    DocFxBuild("./Docfx/docfx.json");
});

Task("DocsServe")
    .IsDependentOn("Docs")
    .Does(() => DocFxServe("./docs"));

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);