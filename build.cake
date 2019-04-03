#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Arguments
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Preparation
var buildDirectory = Directory("./Common/bin") + Directory(configuration);

// Tasks

Task("Clean").Does(() =>
{
  CleanDirectory(buildDirectory);
});

Task("Restore-NuGet-Packages")
  .IsDependentOn("Clean")
  .Does(() =>
{
  NuGetRestore("./Toolbox.sln");
});

Task("Build")
  .IsDependentOn("Restore-NuGet-Packages")
  .Does(() =>
{
  if(IsRunningOnWindows())
  {
    // Use MSBuild
    MSBuild("./Toolbox.sln", settings =>
      settings.SetConfiguration(configuration));
  }
  else
  {
    // Use XBuild
    XBuild("./Toolbox.sln", settings =>
      settings.SetConfiguration(configuration));
  }
});

Task("Run-Unit-Tests")
  .IsDependentOn("Build")
  .Does(() =>
{
  NUnit3("./**/bin/" + configuration + "/Tests.dll", new NUnit3Settings
  {
    // NoResults = true
  });
});

Task("Clean-Artifacts")
  .IsDependentOn("Run-Unit-Tests")
  .Does(() => CleanDirectory("./artifacts"));

Task("NuGetPack")
.IsDependentOn("Clean-Artifacts")
.Does(() =>
{
  NuGetPack($".nuspec", new NuGetPackSettings
  {
    Id = $"Toolbox.Common",
    Version = "0.0.0.1",
    OutputDirectory = "./artifacts",
    Files = new[]
    {
      new NuSpecContent
      {
        Source = $"./Common/bin/Release/Common.dll",
        Target = $"./content/Common",
      }
    },
  });
});

Task("NuGetPush")
.IsDependentOn("NuGetPack")
.Does(() =>
{
  // Get the path to the package.
  var package = "./artifacts/**.nupkg";

 // Push the package.
 NuGetPush(package, new NuGetPushSettings {
     Source = "https://www.nuget.org",
     ApiKey = "oy2nwfqcipasa5rqevbq4j4vhoqwo6gu7g7o5ko2r5epyy"
 });
});

Task("Default")
  .IsDependentOn("NuGetPush")
  .Does(() =>
{
  Information("Finished!");
});

RunTarget(target);