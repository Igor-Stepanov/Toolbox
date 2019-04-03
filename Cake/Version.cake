#addin nuget:?package=semver&version=2.0.4

#load Logging.cake
#load Git.cake

using Semver;

const string _VersionGitTagPrefix = "version/";
const string _BuildNumberGitTagPrefix = "builds/";

static readonly DateTime Epoch = new DateTime(2018, 10, 1, 0, 0, 0, DateTimeKind.Utc);

static class _NextPatchCache
{
  public static int? Major = null;
  public static int? Minor = null;
  public static int? Patch = null;

  public static int CachePatch(int major, int minor, int patch)
  {
    Major = major;
    Minor = minor;
    Patch = patch;

    return patch;
  }
}

string NextSemanticVersionFromGitTags(int major, int minor, string label) =>
  new SemVersion(
    major,
    minor,
    NextSemVerPatchFromGitTags(major, minor),
    label)
    .ToString();

string NextSemanticVersionFromGitTagsRespectingLocalAlphaBuild(int major, int minor) =>
  NextSemanticVersionFromGitTags(
    major,
    minor,
    BuildSystem.IsLocalBuild
      ? "alpha"
      : null
  );

int NextSemVerPatchFromGitTags(int major, int minor)
{
  bool patchWasDeterminedForSuchVersion =
    _NextPatchCache.Patch.HasValue &&
    major == _NextPatchCache.Major &&
    minor == _NextPatchCache.Minor;

  if (patchWasDeterminedForSuchVersion)
  {
    var patch = _NextPatchCache.Patch.Value;
    Information($"Next SemVer patch was already determined for version {major}.{minor}.");
    Information($"Next patch is {patch}.");
    return patch;
  }

  Information($"Determining next SemVer patch for version {major}.{minor}.");
  Information("");

  var tags = GitTags(".").Select(tag => tag.ToString());

  InformationArray("Found following tags in repository:", tags);

  var versions =
    from tag in tags
    let tagString = tag.ToString()
    where tagString.StartsWith($"refs/tags/{_VersionGitTagPrefix}")
    select SemVersion.Parse(tagString.Replace($"refs/tags/{_VersionGitTagPrefix}", ""));

  InformationArray("Parsed following versions from tags:", versions);

  var filteredOrderedVersions =
    Enumerable.ToList
    (
      from version in versions
      where version.Major == major
      where version.Minor == minor
      orderby version.Patch
      select version
    );

  if (filteredOrderedVersions.Any())
  {
    var latestVersion = filteredOrderedVersions.Last();
    var patch = latestVersion.Patch + 1;
    Information($"Picked version {latestVersion} as latest registered in repository.");
    Information($"Next patch is {patch}.");
    return _NextPatchCache.CachePatch(major, minor, patch);
  }
  else
  {
    var patch = 0;
    Information($"No version like {major}.{minor} is found in repository.");
    Information($"Next patch is {patch}.");
    return _NextPatchCache.CachePatch(major, minor, patch);
  }
}

string SemanticVersionToLocalGitTags(string version)
{
  var tag = $"{_VersionGitTagPrefix}{version}";

  MarkCurrentCommitWithGitTag(tag: tag);

  return tag;
}

string SemanticVersionToCentralRepositoryGitTags(string version, string repository)
{
  string tag = SemanticVersionToLocalGitTags(version);

  PushGitTagToCentralRepository(tag: tag, repository: repository);

  return tag;
}

string DirectorySemanticVersionFromCommits(string directoryPath, int major, string label)
{
  int masterCommits = CountGitCommitsInDirectoryOnMaster(directoryPath);
  int branchOnlyCommits = CountGitCommitsInDirectoryExcludingMaster(directoryPath);

  return
    new SemVersion(
      major: masterCommits > 0 ? major : 0,
      minor: masterCommits > 0 ? masterCommits - 1 : 0,
      patch: branchOnlyCommits,
      prerelease: label)
      .ToString();
}

string DirectorySemanticVersionFromCommitsRespectingLocalAlphaBuild(string directoryPath, int major) =>
  DirectorySemanticVersionFromCommits(
    directoryPath,
    major,
    BuildSystem.IsLocalBuild
      ? "alpha"
      : null
  );

int GetLastBuildNumberFromTags()
{
  Information("Getting last build number from git tags...");
  
  var tags = GitTags(".").Select(tag => tag.ToString());

  InformationArray("Found following tags in repository:", tags);
  
  var numbers =
    from tag in tags
    let tagString = tag.ToString()
    where tagString.StartsWith($"refs/tags/{_BuildNumberGitTagPrefix}")
    select int.Parse(tagString.Replace($"refs/tags/{_BuildNumberGitTagPrefix}", ""));
  
  var buildNumber = 0;
  if (numbers.Any())
    buildNumber = numbers.Max(value => value);
  
  Information($"Last build number is {buildNumber}");
  
  return buildNumber;
}

void BuildNumberToCentralRepositoryGitTags(int buildNumber, string repository)
{
  var newTag = $"{_BuildNumberGitTagPrefix}{buildNumber}";
  
  MarkCurrentCommitWithGitTag(tag: newTag);
  PushGitTagToCentralRepository(tag: newTag, repository: repository);
}

string VersionFromGitHash(string commitHash)
{
  if (string.IsNullOrEmpty(commitHash) )
    throw new ArgumentNullException(nameof(commitHash));

  Information($"Commit hash: {commitHash}");

  var length = Math.Min(7, commitHash.Length);;
  commitHash = commitHash.Substring(0, length);

  Information($"Short commit hash: {commitHash}");

  // We must prefix the git hash with a 1
  // If it starts with a zero, when we decimalize it,
  // and later hexify it, we'll lose the zero.
  commitHash = string.Format("1{0}", commitHash);
  int decCommitHash = Convert.ToInt32(commitHash, 16);
  
  var minutesFromEpoch = Math.Round((DateTime.Now - Epoch).TotalMinutes);

  var version = $"{minutesFromEpoch}.{decCommitHash}";
  Information($"Version: {version}");
  return version;
}

string GitHashFromVersion(string version)
{
  if (string.IsNullOrEmpty(version))
    throw new ArgumentNullException(nameof(version));

  Information($"Version: {version}");

  var versionParts = version.Split(new string[] { "." }, StringSplitOptions.None);
  if (versionParts.Length != 2)
    throw new ArgumentException($"Can't separate verstion. Argument {nameof(version)} is in invalid format.");
  
  // Get second part of version with commit hash in decimal
  var decCommitHash = int.Parse(versionParts[1]);
  Information($"Dirty decimal commit hash: {decCommitHash}");

  string hexCommitHash = decCommitHash.ToString("X");

  // Remove '1' from begining of hex commit hash value
  hexCommitHash = hexCommitHash.Substring(1).ToLower();
  Information($"Commit hash: {hexCommitHash}");
  return hexCommitHash;
}