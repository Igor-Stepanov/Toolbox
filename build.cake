#load ./Cake/Version.cake
#load ./Cake/Build.cake
// Arguments
var target = Argument("target", "Default");
const int MajorVersion = 1;
const int MinorVersion = 0;
string Version() => DirectorySemanticVersionFromCommitsRespectingLocalAlphaBuild("./Common", MajorVersion);

// Tasks
Task("Clean-Artifacts").Does(() => CleanDirectory("./artifacts"));
//Task("Build").Does(() => BuildSolution("./Toolbox.sln"));
Task("Run-Unit-Tests").Does(() => NUnit3("./Tests/bin/ReleaseTests.dll", new NUnit3Settings { NoResults = false }));

Task("NuGetPack").Does(() =>
{
  NuGetPack($".nuspec", new NuGetPackSettings
  {
    Id = $"Toolbox.Common",
    Version = "1.0.0.0",
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
  //.IsDependentOn("Clean-Artifacts")
  //.IsDependentOn("Build")
  //.IsDependentOn("Run-Unit-Tests")
  //.IsDependentOn("NuGetPack")
  //.IsDependentOn("NuGetPush")
  .Does(() =>
{  
  var v1 = Version();
  var v2 = Version();
  Information("\nSuccess");
});

Task("Hello").Does(() => 
{
  var nuGetPackSettings = new NuGetPackSettings
	{
		OutputDirectory = @"./artifacts/",
		IncludeReferencedProjects = true,
		Properties = new Dictionary<string, string>
		{
			{ "Configuration", "Release" }
		}
	};

  MSBuild("./Common/Common.csproj", new MSBuildSettings().SetConfiguration("Release"));
  NuGetPack("./Common/Common.csproj", nuGetPackSettings);  
});

RunTarget("Hello");
