#load Logging.cake
#load Slack.cake
#load Git.cake

class BranchInformation
{
  public string Remote;
  public string BranchName;
  public string Revision;
  public string CommitMessage;
  public string CommiterName;
  public string CommiterEmail;
  public string LastCommitAge;
  public DateTime LastCommitDate;
  
  public static BranchInformation Parse(string[] parts)
  {
    var branchFullName = parts[0].Trim();
    var indexOfFirstSeparator = branchFullName.IndexOf("/");
    
    var remoteName = branchFullName.Substring(0, indexOfFirstSeparator);
    var branchName = branchFullName.Substring(indexOfFirstSeparator + 1);

    return new BranchInformation {
      Remote = remoteName,
      BranchName = branchName,
      Revision = parts[1],
      CommitMessage = parts[2],
      CommiterName = parts[3],
      CommiterEmail = parts[4],
      LastCommitAge = parts[5],
      LastCommitDate = DateTime.Parse(parts[6])
    };
  }

  public override string ToString()
  {
    return 
    "----------------------------------------\r\n" +
    $"Remote: {Remote}\r\n" +
    $"BranchName: {BranchName}\r\n" +
    $"Revision: {Revision}\r\n" +
    $"CommitMessage: {CommitMessage}\r\n" +
    $"CommiterName: {CommiterName}\r\n" +
    $"CommiterEmail: {CommiterEmail}\r\n" +
    $"LastCommitAge: {LastCommitAge}\r\n" +
    $"LastCommitDate: {LastCommitDate}\r\n" +
    "----------------------------------------\r\n";
  }
}

public class MergedBranchesNotifySettings
{
  public const string Separator = "~~~";
  public const string RemoteName = "central";

  public string Repository { get; private set; }
  public string RepositoryPath { get; private set; }

  public string TargetBranch { get; private set; }

  public string[] ExcludedBranches { get; private set; }
  public string[] ExcludedFolders { get; private set; }

  public string SlackChannel { get; private set; }
  public string ForceSlackChannel { get; private set; }

  public int DaysForBranchBecomesOld { get; private set; }
  public string[] ReportReceivers { get; private set; }

  public MergedBranchesNotifySettings ForRepository(string repository, string path = ".")
  {
    Repository = repository;
    RepositoryPath = path;

    return this;
  }

  public MergedBranchesNotifySettings WithTargetBranch(string branchName)
  {
    TargetBranch = branchName;
    return this;
  }

  public MergedBranchesNotifySettings ExceptBranches(string[] branches)
  {
    ExcludedBranches = branches;
    return this;
  }

  public MergedBranchesNotifySettings ExceptFolders(string[] folders)
  {
    ExcludedFolders = folders;
    return this;
  }

  public MergedBranchesNotifySettings NotifyTo(string channel)
  {
    SlackChannel = channel;
    return this;
  }

  public MergedBranchesNotifySettings ForceNotifyTo(string channel)
  {
    ForceSlackChannel = channel;
    return this;
  }

  public MergedBranchesNotifySettings NotifyWhenBranchOlderThen(int count)
  {
    DaysForBranchBecomesOld = count;
    return this;
  }

  public MergedBranchesNotifySettings WithReportReceivers(string[] receivers)
  {
    ReportReceivers = receivers;
    return this;
  }

  public void Validate(ICakeContext context)
  {
    // Validations
    if (string.IsNullOrWhiteSpace(Repository))
      throw new ArgumentException("Repository doesn't set.");

    if (string.IsNullOrWhiteSpace(RepositoryPath))
      throw new ArgumentException("Repository path doesn't set");

    if (!context.DirectoryExists(RepositoryPath))
      throw new ArgumentException($"Failed to find directory by path: {RepositoryPath}");

    if (string.IsNullOrWhiteSpace(TargetBranch))
      throw new ArgumentException("Target branch doesn't set.");

    if (ExcludedBranches == null)
      throw new ArgumentNullException("Except branches doesn't set.");

    if (string.IsNullOrWhiteSpace(SlackChannel) && string.IsNullOrWhiteSpace(ForceSlackChannel))
      throw new ArgumentException("Slack channel for notifications doesn't set.");
  }
}

void NotifyAboutOldBranches(MergedBranchesNotifySettings settings)
{
  var oldBranches = CollectBranches(settings, GetBranchesBuilder(settings.TargetBranch, false));
  if (oldBranches == null || oldBranches.Length == 0)
  {
    Information("No branches to process. Exit.");
    return;
  }

  var filteredBranches = oldBranches
    .Where(x => (DateTime.Now - x.LastCommitDate).TotalDays >= settings.DaysForBranchBecomesOld)
    .GroupBy((branchInfo) => branchInfo.CommiterEmail)
    .OrderByDescending(x => x.Count());

  foreach(var branchesByUser in filteredBranches)
  {
    var commiterEmail = branchesByUser.Key;
    var message = __BuildOldBranchesMessage(commiterEmail, branchesByUser.OrderBy(x => x.LastCommitDate)).ToString();

    var privateMessageDelivered = false;
    var commiterSlackName = __SlackNameFromEmail(commiterEmail);
    if (commiterSlackName != null)
      privateMessageDelivered = SendSlackMessage(commiterSlackName, message);

    if (!privateMessageDelivered)
    {
      var delivered = SendSlackMessage(
        to: settings.ForceSlackChannel ?? settings.SlackChannel,
        message:
        $"Мне не удалось лично уведомить уведомить коммитера {commiterEmail ?? "с неизвестным логином"} про устаревшие бранчи, " +
        "наверное, он сменил имя или не указал корпоративную почту в настройках Git'а, или его почта не совпадает с ником в Slack.\r\n" +
        "Вы уж как нибудь ему скажите про то, что стоит держать информацию в актуальном состоянии. И про устарвешие бранчи тоже напомните.\r\n" +
        message.ToString());
      
      if (!delivered)
        Error("Failed to notify users about merged branches.");
    }

    var receivers = settings.ReportReceivers;
    if (receivers != null)
      Array.ForEach(receivers, x => SendSlackMessage(x, message));
  }
}

void NotifyAboutMergedBranches(MergedBranchesNotifySettings settings)
{
  if (settings == null)
    throw new ArgumentNullException(nameof(settings));

  settings.Validate(Context);

  if (_SlackSettings == null)
    throw new Exception("Configure slack by calling method: 'ConfigureSlack' first.");
    
  // Add central repository
  AddRemoteRepository(MergedBranchesNotifySettings.RemoteName, settings.Repository, settings.RepositoryPath);

  // Fetch from central with prune (delete removed branches)
  GitFetch(settings.RepositoryPath, true);

  // Collect merged branches collection
  var mergedBranches = CollectBranches(settings, GetBranchesBuilder(settings.TargetBranch));
  if (mergedBranches == null || mergedBranches.Length == 0)
  {
    Information("No merged branches to process. Exit.");
    return;
  }

  // Apply filters
  // Filter by except branches
  var filteredBranches = mergedBranches.Where(branchInfo => !settings.ExcludedBranches.Contains(branchInfo.BranchName));
  // Filter by except folders
  if (settings.ExcludedFolders != null && settings.ExcludedFolders.Length > 0)
    filteredBranches = filteredBranches.Where(branchInfo => settings.ExcludedFolders.All(folder => !branchInfo.BranchName.StartsWith(folder)));

  foreach(var branchesByUser in filteredBranches.GroupBy((branchInfo) => branchInfo.CommiterEmail))
  {
    var commiterEmail = branchesByUser.Key;

    var message = __BuildMessage(commiterEmail, branchesByUser);
    var commiterSlackName = __SlackNameFromEmail(commiterEmail);

    // Try to send personal message
    var privateMessageDelivered = false;
    if (commiterSlackName != null)
    {
      privateMessageDelivered = SendSlackMessage(
        to: settings.ForceSlackChannel ?? commiterSlackName,
        message: message.ToString()
      );
    }

    // Send message to default group
    if (!privateMessageDelivered)
    {
      var delivered = SendSlackMessage(
        to: settings.ForceSlackChannel ?? settings.SlackChannel,
        message:
        $"Мне не удалось лично уведомить уведомить коммитера {commiterEmail ?? "с неизвестным логином"} про не удалённые им бранчи, " +
        $"которые были успешно замерджены в '{settings.TargetBranch}'. " +
        "Наверное, он сменил имя или не указал корпоративную почту в настройках Git'а, или его почта не совпадает с ником в Slack.\r\n" +
        "Вы уж как нибудь ему скажите про то, что стоит держать информацию в актуальном состоянии. И про не удаленные бранчи тоже напомните.\r\n" +
        message.ToString());

      if (!delivered)
        Error("Failed to notify users about merged branches.");
    }
  }
}

BranchInformation[] CollectBranches(MergedBranchesNotifySettings settings, ProcessArgumentBuilder arguments)
{ 
  if (settings == null)
    throw new ArgumentNullException(nameof(settings));

  Information($"Collecting information about merged to '{settings.TargetBranch}' branches...");
  
  IEnumerable<string> output;
  int exitCode = StartProcess("git", new ProcessSettings
  {
    WorkingDirectory = settings.RepositoryPath,
    RedirectStandardOutput = true,
    Arguments = arguments       
  }, out output);

  if (exitCode != 0)
    throw new Exception($"Failed to collect info about merged branches.");
  
  var lines = output.ToArray();
  
  InformationArray("Git command output:", lines);
  Information("---------------------------------");
  
  return ParseBranchInfo(lines);
}

private ProcessArgumentBuilder GetBranchesBuilder(string targetBranch, bool includeMerged = true)
{
  var builder = new ProcessArgumentBuilder()
    .Append("for-each-ref")
    .Append("--sort=-committerdate")
    .Append("--sort=-authoremail")
    .Append("refs/remotes/" + MergedBranchesNotifySettings.RemoteName);

    if (includeMerged)
      builder.Append("--merged");

    builder.Append($"{MergedBranchesNotifySettings.RemoteName}/{targetBranch}")
    .Append(
      $"--format=\"%(HEAD) " +
      $"%(refname:short){MergedBranchesNotifySettings.Separator}" +
      $"%(objectname:short){MergedBranchesNotifySettings.Separator}" +
      $"%(contents:subject){MergedBranchesNotifySettings.Separator}" +
      $"%(authorname){MergedBranchesNotifySettings.Separator}" +
      $"%(authoremail){MergedBranchesNotifySettings.Separator}" +
      $"%(committerdate:relative){MergedBranchesNotifySettings.Separator}" +
      $"%(committerdate:short)\"");
    
  return builder;
}

private BranchInformation[] ParseBranchInfo(string[] lines)
{
  if (lines == null)
    throw new ArgumentNullException(nameof(lines));
    
  if (lines.Length == 0)
    return null;
    
  Information("Parsing branch information from git command output...");
    
  var infos = new BranchInformation[lines.Length];
  for(int i = 0; i < lines.Length; i++)
  {
    var parts = lines[i].Split(new string[] { MergedBranchesNotifySettings.Separator }, StringSplitOptions.None);
    infos[i] = BranchInformation.Parse(parts);
  }

  InformationArray("Parsed information:", infos);
    
  return infos;
}

static string __BuildMessage(string commiterEmail, IEnumerable<BranchInformation> branchesInfo)
{
  string commiterName = null;

  var gitCommand = new StringBuilder("git push origin --delete");
  var branchesInfoMessage = new StringBuilder();

  foreach(var branchInfo in branchesInfo)
  {
    // get the newest name of commiter
    commiterName = commiterName ?? branchInfo.CommiterName;

    branchesInfoMessage.AppendLine($"\t`{branchInfo.LastCommitAge} ({branchInfo.Revision}) {branchInfo.BranchName}`");
    gitCommand.Append($" {branchInfo.BranchName}");
  }
  
  var message = new StringBuilder();
  message.AppendLine($"{commiterName} {commiterEmail}");
  message.AppendLine(branchesInfoMessage.ToString());
  message.AppendLine();
  message.AppendLine("```" + gitCommand.ToString() + "```");
  return message.ToString();
}

static string __BuildOldBranchesMessage(string commiterEmail, IEnumerable<BranchInformation> branchesInfo)
{
  string commiterName = null;
  var branchesInfoMessage = new StringBuilder();

  foreach(var branchInfo in branchesInfo)
  {
    commiterName = commiterName ?? branchInfo.CommiterName;
    branchesInfoMessage.AppendLine($"> {branchInfo.LastCommitAge} ({branchInfo.Revision})\r\n> {branchInfo.BranchName}\r\n");
  }
  
  var email = commiterEmail
    .Replace("<", string.Empty)
    .Replace(">", string.Empty);

  var message = new StringBuilder();
  message.AppendLine($"<mailto:{email}|{commiterName}>, вот ветки в которых твой коммит был последним и уже давно не наблюдается никаких изменений (возможно их стоит удалить или смержить?):");
  message.AppendLine(branchesInfoMessage.ToString());
  return message.ToString();
}

static string __SlackNameFromEmail(string authorEmail)
{
  var emailUser = authorEmail?.ToLower()?
    .Replace("<", "")
    .Replace(">", "")
    .Replace("@plarium.com", "");

  return !string.IsNullOrWhiteSpace(emailUser) ? $"@{emailUser}" : null;
}