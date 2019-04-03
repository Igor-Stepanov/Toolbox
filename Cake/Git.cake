#addin nuget:?package=Cake.Git&version=0.18

string CurrentGitBranch() => GitBranchCurrent(DirectoryPath.FromString(".")).FriendlyName;

string CurrentGitCommitSha() => GitLogTip(DirectoryPath.FromString(".")).Sha;

string CurrentGitCommitMessage() => GitLogTip(DirectoryPath.FromString(".")).Message;

void MarkCurrentCommitWithGitTag(string tag)
{
  Information($"Adding tag {tag} for current commit...");
  GitTag(".", tag);
}

void GitPush(string repositoryName, string repositoryDirectoryPath = ".")
{
  if (repositoryName == null)
      throw new ArgumentNullException(nameof(repositoryName));
      
  if (repositoryDirectoryPath == null)
        throw new ArgumentNullException(nameof(repositoryDirectoryPath));
    
  if (!DirectoryExists(repositoryDirectoryPath))
      throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");
      
  Information($"Push to remote {repositoryName} repository by path: '{repositoryDirectoryPath}'");

  var exitCode = StartProcess("git", new ProcessSettings
  {
      WorkingDirectory = repositoryDirectoryPath,
      Arguments = new ProcessArgumentBuilder()
        .Append("push")
        .Append(repositoryName)
  });
  
  if (exitCode != 0)
    throw new Exception("Failed to push to repository.");
}

void GitFetch(string repositoryDirectoryPath = ".", bool withPrune = false)
{     
  if (repositoryDirectoryPath == null)
        throw new ArgumentNullException(nameof(repositoryDirectoryPath));
    
  if (!DirectoryExists(repositoryDirectoryPath))
      throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");
      
  Information($"Fetch all info for repository by path: '{repositoryDirectoryPath}'" + (withPrune ? " with prune" : ""));

  var arguments = new ProcessArgumentBuilder()
        .Append("fetch")
        .Append("--all");

  if (withPrune)
    arguments.Append("--prune");

  var exitCode = StartProcess("git", new ProcessSettings
  {
      WorkingDirectory = repositoryDirectoryPath,
      Arguments = arguments
  });
  
  if (exitCode != 0)
    throw new Exception($"Failed to fetch info with code {exitCode}.");
}

void GitFetchBranch(string repository, string remoteName, string localName, string repositoryDirectoryPath = ".")
{ 
  if (String.IsNullOrEmpty(repository) )
    throw new ArgumentNullException(nameof(repository));

  if (String.IsNullOrEmpty(remoteName) )
    throw new ArgumentNullException(nameof(remoteName));

  if (String.IsNullOrEmpty(localName) )
    throw new ArgumentNullException(nameof(localName));

  if (repositoryDirectoryPath == null)
    throw new ArgumentNullException(nameof(repositoryDirectoryPath));
    
  if (!DirectoryExists(repositoryDirectoryPath))
    throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");
      
  Information($"Fetch branch '{remoteName}:{localName}' from repo '{repository}' by path: '{repositoryDirectoryPath}'");

  var exitCode = StartProcess("git", new ProcessSettings
  {
      WorkingDirectory = repositoryDirectoryPath,
      Arguments = new ProcessArgumentBuilder()
        .Append("fetch")
        .Append(repository)
        .Append($"{remoteName}:{localName}")
  });
  
  if (exitCode != 0)
    throw new Exception($"Failed to fetch branch with code {exitCode}.");
}

void PushGitTagToCentralRepository(string tag, string repository)
{
  AddCentralRepository(repository);

  Information($"Pushing tag {tag} to central repository...");
  
  int exitCode = StartProcess("git", new ProcessSettings
  {
    Arguments =
      new ProcessArgumentBuilder()
        .Append("push")
        .Append("central")
        .Append(tag)
  });

  if (exitCode != 0)
    throw new Exception($"Failed to push tag {tag} to central repository.");
}

void SetGitBranchToCommit(string branch, string commit, string repositoryDirectoryPath = ".")
{
  if (repositoryDirectoryPath == null)
    throw new ArgumentNullException(nameof(repositoryDirectoryPath));
  
  if (!DirectoryExists(repositoryDirectoryPath))
    throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");

  Information($"Moving '{branch}' branch tip to commit {commit} for repository by path {repositoryDirectoryPath}...");

  int exitCode = StartProcess("git", new ProcessSettings
  {
    WorkingDirectory = repositoryDirectoryPath,
    Arguments =
      new ProcessArgumentBuilder()
        .Append("branch")
        .Append("-f")
        .Append(branch)
        .Append(commit)
  });

  if (exitCode != 0)
    throw new Exception($"Failed to move {branch} branch tip to commit {commit}.");
}

void ResetCurrentGitBranchTipToOtherBranch(string otherBranch)
{
  Information($"Resetting current branch tip to {otherBranch} branch...");

  int exitCode = StartProcess("git", new ProcessSettings
  {
    Arguments =
      new ProcessArgumentBuilder()
        .Append("reset")
        .Append("--hard")
        .Append(otherBranch)
  });

  if (exitCode != 0)
    throw new Exception($"Failed to reset current branch tip to {otherBranch} branch.");
}

void ForcePushCurrentGitBranchToCentralRepository(string repository)
{
  AddCentralRepository(repository);

  Information("Forcing push of current branch to central repository...");

  int exitCode = StartProcess("git", new ProcessSettings
  {
    Arguments =
      new ProcessArgumentBuilder()
        .Append("push")
        .Append("central")
        .Append("-f")
  });

  if (exitCode != 0)
    throw new Exception("Failed to force push of current branch to central repository.");
}

void PushGitBranchToCentralRepository(string branch, string repository, string repositoryDirectoryPath = ".")
{
  if (repositoryDirectoryPath == null)
    throw new ArgumentNullException(nameof(repositoryDirectoryPath));
  
  if (!DirectoryExists(repositoryDirectoryPath))
    throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");

  AddCentralRepository(repository, repositoryDirectoryPath);

  Information($"Pushing '{branch}' branch for repo by path '{repositoryDirectoryPath}' to central repository...");

  int exitCode = StartProcess("git", new ProcessSettings
  {
    WorkingDirectory = repositoryDirectoryPath,
    Arguments =
      new ProcessArgumentBuilder()
        .Append("push")
        .Append("central")
        .Append(branch)
  });

  if (exitCode != 0)
    throw new Exception($"Failed to push {branch} branch to central repository.");
}

bool HasUncommittedGitChanges(string directoryPath = ".") =>
  !string.IsNullOrEmpty(ShortGitStatus(directoryPath));

string ShortGitStatus(string directoryPath = ".")
{
  Information($"Retrieving short git status for {directoryPath} directory...");

  IEnumerable<string> output;

  StartProcess(
    "git",
    new ProcessSettings
    {
      RedirectStandardOutput = true,
      Arguments =
        new ProcessArgumentBuilder()
          .Append("status")
          .Append(directoryPath)
          .Append("--short"),
    },
    out output);

  return string.Join(Environment.NewLine, output);
}

int CountGitCommitsInDirectory(string directoryPath)
{
  Information($"Counting commits in {directoryPath} directory...");

  IEnumerable<string> output;

  StartProcess(
    "git",
    new ProcessSettings
    {
      RedirectStandardOutput = true,
      Arguments =
        new ProcessArgumentBuilder()
          .Append("log")
          .Append("--pretty=format:'%H'")
          .Append("--")
          .AppendQuoted(directoryPath),
    },
    out output);

  var commitCount = output.Count();

  Information($"Found {commitCount} commit(s).");

  return commitCount;
}

int CountGitCommitsInDirectoryOnMaster(string directoryPath)
{
  Information($"Counting commits on master branch in {directoryPath} directory...");

  IEnumerable<string> output;

  StartProcess(
    "git",
    new ProcessSettings
    {
      RedirectStandardOutput = true,
      Arguments =
        new ProcessArgumentBuilder()
          .Append("log")
          .Append("master")
          .Append("--pretty=format:'%H'")
          .Append("--")
          .AppendQuoted(directoryPath),
    },
    out output);

  var commitCount = output.Count();

  Information($"Found {commitCount} commit(s).");

  return commitCount;
}

int CountGitCommitsInDirectoryExcludingMaster(string directoryPath)
{
  Information($"Counting commits excluding master branch in {directoryPath} directory...");

  IEnumerable<string> output;

  StartProcess(
    "git",
    new ProcessSettings
    {
      RedirectStandardOutput = true,
      Arguments =
        new ProcessArgumentBuilder()
          .Append("log")
          .Append("master..")
          .Append("--pretty=format:'%H'")
          .Append("--")
          .AppendQuoted(directoryPath),
    },
    out output);

  var commitCount = output.Count();

  Information($"Found {commitCount} commit(s).");

  return commitCount;
}

string CurrentGitCommitInfo()
{
  var lastCommit = GitLogTip(".");
  return string.Format(@"Last commit {0}
    Short message: {1}
    Author:        {2}
    Authored:      {3:yyyy-MM-dd HH:mm:ss}
    Committer:     {4}
    Committed:     {5:yyyy-MM-dd HH:mm:ss}",
    lastCommit.Sha,
    lastCommit.MessageShort,
    lastCommit.Author.Name,
    lastCommit.Author.When,
    lastCommit.Committer.Name,
    lastCommit.Committer.When
    );
}

string MessageOfLastGitCommitInDirectory(string directoryPath)
{
  Information($"Reading last commit in directory {directoryPath}...");

  IEnumerable<string> output;

  StartProcess(
    "git",
    new ProcessSettings
    {
      RedirectStandardOutput = true,
      Arguments =
        new ProcessArgumentBuilder()
          .Append("log")
          .Append("-1")
          .Append("--pretty=format:%s")
          .Append("--")
          .Append(directoryPath),
    },
    out output);

  return output.SingleOrDefault();
}

void AddRemoteRepository(string remoteName, string repository, string repositoryDirectoryPath = ".")
{
  if (remoteName == null)
    throw new ArgumentNullException(nameof(remoteName));

  if (repositoryDirectoryPath == null)
    throw new ArgumentNullException(nameof(repositoryDirectoryPath));

  if (repository == null)
    throw new ArgumentNullException(nameof(repository));
  
  if (!DirectoryExists(repositoryDirectoryPath))
    throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");

  Information($"Add for repo '{repositoryDirectoryPath}' remote '{remoteName}' repository: {repository}");

  StartProcess("git", new ProcessSettings
  {
    WorkingDirectory = repositoryDirectoryPath,
    Arguments = new ProcessArgumentBuilder()
      .Append("remote")
      .Append("add")
      .Append(remoteName)
      .Append(repository)
  });
}

void AddCentralRepository(string repository, string repositoryDirectoryPath = ".")
{
  AddRemoteRepository("central", repository, repositoryDirectoryPath);
}

bool GitHasStagedFiles(string repositoryDirectoryPath = ".")
{
    if (repositoryDirectoryPath == null)
        throw new ArgumentNullException(nameof(repositoryDirectoryPath));
    
    if (!DirectoryExists(repositoryDirectoryPath))
        throw new ArgumentException($"Failed to find directory by path: {repositoryDirectoryPath}");

    Information($"Get staged index by path: '{repositoryDirectoryPath}'");

    IEnumerable<string> stagedFiles;
    StartProcess("git", new ProcessSettings
    {
        WorkingDirectory = repositoryDirectoryPath,
        RedirectStandardOutput = true,
        Arguments = new ProcessArgumentBuilder()
            .Append("diff")
            .Append("--name-only")
            .Append("--cached")
    },
    out stagedFiles);
  
    return stagedFiles != null && stagedFiles.Any();
}

void GitCheckoutRepository(string repository, string repositoryDirectoryPath, string branch)
{
  if (string.IsNullOrEmpty(repository))
    throw new ArgumentNullException(nameof(repository));

  if (repositoryDirectoryPath == null)
    throw new ArgumentNullException(nameof(repositoryDirectoryPath));

  if (string.IsNullOrEmpty(branch))
    throw new ArgumentNullException(nameof(branch));

  Information("Run repository checkout...");
  
  var absoluteRepositoryPath = MakeAbsolute(Directory(repositoryDirectoryPath)).ToString();

  if (!DirectoryExists(absoluteRepositoryPath))
  {
    Information("Repository folder doesn't exist. Create it and cloning...");
    GitClone(repository, absoluteRepositoryPath, 
      new GitCloneSettings
      { 
        BranchName = branch
      });
  }
  else if (!GitIsValidRepository(absoluteRepositoryPath))
    throw new Exception("Repository directory exists, but it's not a valid git repository.");

  // Fetch all branches
  GitFetch(absoluteRepositoryPath);

  Information("Sync current branch with remote repository");
  GitReset(absoluteRepositoryPath, GitResetMode.Hard, $"origin/{branch}");
}