//#load ./Cake/Version.cake
#load ./Cake/Build.cake
#tool "nuget:?package=NUnit.ConsoleRunner"
// Arguments
var target = Argument("target", "Default");

// Preparation
var artifacts  = Directory("./artifacts/");
var solution = File("./Toolbox.sln");
var root = MakeAbsolute(Directory("./")).FullPath;

// Tasks
Task("Clean").Does(() => CleanDirectory(artifacts));
Task("RestoreNuGet").Does(() => NuGetRestore(solution));
Task("Build").Does(() => BuildSolution(solution));
Task("BuildTests").Does(() =>
{
	var projects = ParseSolution(solution).Projects;

	foreach(var project in projects.Where(x => x.Name.EndsWith("Tests")))
	{
    Information($"Building Test: {project.Name}");
    
    MSBuild(project.Path, new MSBuildSettings()
      .SetConfiguration("Debug")
      .SetMSBuildPlatform(MSBuildPlatform.Automatic)
      .SetVerbosity(Verbosity.Minimal)
      .WithProperty("SolutionDir", root)
      .WithProperty("OutDir", $"{artifacts}/_tests/{project.Name}/"));
	}
});

Task("RunTests").Does(() => NUnit3(@"./artifacts/_tests/**/*Tests.dll", new NUnit3Settings { NoResults = false }));

Task("NuGetPack").Does(() =>
{
  var nuGetPackSettings = new NuGetPackSettings
	{
		OutputDirectory = @"./artifacts/",
		IncludeReferencedProjects = true,
    Id = "Toolbox.Common",
    Title = "Toolbox.Common",
		Properties = new Dictionary<string, string>
		{
			{ "Configuration", "Release" }
		}
	};

  MSBuild("./Common/Common.csproj", new MSBuildSettings().SetConfiguration("Release"));
  NuGetPack("./Common/Common.csproj", nuGetPackSettings);
});

Task("NuGetPush").Does(() =>
{
  NuGetPush("./artifacts/**.nupkg", new NuGetPushSettings {
     Source = "https://www.nuget.org",
     ApiKey = "oy2mzdgbq6a5gxpglojqrmsybfqyviluxk5erkms7s7vu4"
 });
});
Task("Default")
  .IsDependentOn("Clean")
  .IsDependentOn("RestoreNuGet")
  .IsDependentOn("Build")
  .IsDependentOn("BuildTests")
  .IsDependentOn("RunTests")
  .IsDependentOn("NuGetPack")
  //.IsDependentOn("NuGetPush")
  .Does(() => Information("Finished!"));

RunTarget(target);