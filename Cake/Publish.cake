#addin nuget:?package=Flurl&version=2.7.1
#addin nuget:?package=Flurl.Http&version=2.3.2
#addin nuget:?package=Newtonsoft.Json

#load Git.cake
#load Logging.cake

using Flurl;
using Flurl.Http;

const string AltoolDirectoryPath = @"/Applications/Xcode.app/Contents/Applications/Application Loader.app/Contents/Frameworks/ITunesSoftwareService.framework/Support/";

async Task PublishToPlariumFlightAsync(string packagePath, string category,
  string description = null, string changelog = null, bool stagingServer = false)
{
  await PublishToPlariumFlightAsyncWithResult(packagePath, category, description, changelog, stagingServer);
}

async Task<string> PublishToPlariumFlightAsyncWithResult(string packagePath, string category,
  string description = null, string changelog = null, bool stagingServer = false)
{
  Information("Publishing to Plarium Flight...");

  if (string.IsNullOrWhiteSpace(changelog))
  {
    Information("No changelog supplied, using branch information.");
    changelog = $"Branch: {CurrentGitBranch()}";
  }

  string apiUrl =
    stagingServer
      ? "https://staging-wi.x-plarium.com/flight/api/package"
      : "https://pf.x-plarium.com/api/package";

  var response = await
    apiUrl
      .PostMultipartAsync(content => content
        .AddFile("file", packagePath)
        .AddStringParts(new
        {
          token = "qp4eULFe4DbVgYtsE6eCaSQN",
          category = category.Replace('/', '\\'),
          description = description ?? string.Empty,
          changelog = changelog,
        }))
      .ReceiveString();

  Information($"Received response: {response}");
  return response;
}

void PublishToTestFlight(string ipaPath, string email, string alttoolPath = null)
{
  Information("Publishing to Test Flight...");

  var arguments =
    new ProcessArgumentBuilder()
      .Append("--upload-app")
      .Append("-f").AppendQuoted(ipaPath)
      .Append("-t").Append("ios")
      .Append("-u").Append(email)
      .Append("-p").Append($"@keychain:\"Application Loader: {email}\"");

  var altoolDirectory = alttoolPath ?? AltoolDirectoryPath;
  var altool = altoolDirectory + "altool";
  var exitCode = StartProcess(altool, new ProcessSettings
  {
      Arguments = arguments
  });
  
  if (exitCode != 0)
    throw new Exception("Failed to push to repository.");
}

void PushNuGetPackageToProGet(string packagePath, string apiKey) =>
  NuGetPush(packagePath, new NuGetPushSettings
  {
    Source = "https://proget.x-plarium.com/nuget/plarium",
    ApiKey = apiKey,
  });

bool PushNuGetPackageToProGetIfNew(
  string packageId, string packageVersion, string packagePath, string apiKey)
{
  Information($"Preparing to push NuGet package {packagePath} to ProGet...");

  Information($"Checking existing versions of package {packageId}...");

  var versions =
    NuGetList(
      packageId,
      new NuGetListSettings
      {
        Source = new [] { "https://proget.x-plarium.com/nuget/plarium" },
        AllVersions = true,
        Prerelease = true,
      })
    .Where(package => package.Name == packageId)
    .Select(package => package.Version);

  InformationArray("Found following versions:", versions);

  bool alreadyPushed = versions.Contains(packageVersion);

  if (alreadyPushed)
  {
    Information("Package with same version is already pushed, there is nothing new to push now.");
    return false;
  }

  Information("Package is new. Pushing it...");

  PushNuGetPackageToProGet(packagePath: packagePath, apiKey: apiKey);

  return true;
}
