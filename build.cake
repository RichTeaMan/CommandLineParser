#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=vswhere"
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
    NuGetRestore("./CommandLineParser.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {

        DirectoryPath vsLatest  = VSWhereLatest();
        FilePath msBuildPathX64 = (vsLatest==null)
                                ? null
                                : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

        Information($"MS Build Path: {msBuildPathX64}");
        MSBuild("CommandLineParser.sln", new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.MSIL,
        ToolPath = msBuildPathX64
        });
    }
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("CommandLineParser.Tests/CommandLineParser.Tests.csproj");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);