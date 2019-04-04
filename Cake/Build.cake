//using System.Dynamic;

void BuildSolution(string solutionPath, string configuration = "Release", string customMSBuildPath = null, string defineConstants = null)
{
  MSBuild(solutionPath, settings =>
    {
      settings.SetConfiguration(configuration);
      if (customMSBuildPath != null)
        settings.ToolPath = customMSBuildPath;

      if (defineConstants != null)
        settings.WithProperty("DefineConstants", new[] { defineConstants });
    });
}

/*void BuildUnityProject(
  string projectPath,
  string unityPath,
  string executeMethod,
  Action<dynamic> fillExecuteMethodArguments,
  string buildTarget = null,
  string logFile = null)
{
  dynamic executeMethodArguments = new ExpandoObject();
  fillExecuteMethodArguments(executeMethodArguments);

  BuildUnityProject(
    projectPath,
    unityPath,
    executeMethod,
    executeMethodArguments,
    buildTarget: buildTarget,
    logFile: logFile);
}

void BuildUnityProject(
  string projectPath,
  string unityPath,
  string executeMethod,
  IDictionary<string, object> executeMethodArguments,
  string buildTarget = null,
  string logFile = null)
{
  var arguments = new ProcessArgumentBuilder();

  arguments
    .Append("-quit")
    .Append("-batchmode");

  if (buildTarget != null)
  {
    arguments.Append("-buildTarget").Append(buildTarget);
  }

  if (logFile != null)
  {
    arguments.Append("-logFile").AppendQuoted(logFile);
  }

  arguments.Append("-projectPath").AppendQuoted(MakeAbsolute(Directory(projectPath)).ToString());
  arguments.Append("-executeMethod").Append(executeMethod);

  foreach (var argument in executeMethodArguments)
  {
    arguments.Append($"-{argument.Key}={argument.Value}");
  }

  Information($"Running Unity build...");
  Information($"Unity path: {unityPath}");
  Information($"Arguments: {arguments.Render()}");

  StartProcess(File(unityPath), new ProcessSettings { Arguments = arguments });
}

int LaunchUnity(
    string unityPath,
    string projectPath,
    string executeMethod = null,
    string buildTarget = null,
    string logFile = null,
    Action<dynamic> fillExecuteMethodArguments = null,
    string[] additionalUnityArguments = null,
    bool quit = true,
    bool nographics = true,
    bool batchmode = true)
{
    // Build unity args
    var arguments = new ProcessArgumentBuilder();

    if (batchmode)
        arguments.Append("-batchmode");

    if (nographics)
        arguments.Append("-nographics");

    if (quit)
        arguments.Append("-quit");

    // Required args
    arguments.Append("-projectPath").AppendQuoted(MakeAbsolute(Directory(projectPath)).ToString());
    arguments.Append("-logFile").AppendQuoted(MakeAbsolute(File(logFile)).ToString());

    // Optional args
    if (buildTarget != null)
        arguments.Append("-buildTarget").Append(buildTarget);

    if (executeMethod != null)
        arguments.Append("-executeMethod").Append(executeMethod);

    if (additionalUnityArguments != null)
        foreach (var arg in additionalUnityArguments)
            arguments.Append(arg);

    if (fillExecuteMethodArguments != null)
    {
        dynamic executeMethodArguments = new ExpandoObject();
        fillExecuteMethodArguments(executeMethodArguments);

        foreach (var argument in executeMethodArguments)
            arguments.Append($"-{argument.Key}={argument.Value}");
    }

    Information($"Launch Unity...");
    Information($"Unity path: {unityPath}");
    Information($"Arguments: {arguments.Render()}");
  
    var launchResult = StartProcess(File(unityPath), new ProcessSettings { Arguments = arguments });

    Information($"Launch Unity finished with result: {launchResult}");

    return launchResult;
}

void BuildXcodeProject(string projectPath, string archivePath, string exportPath,
  string derivedDataPath, string exportOptionsPlistPath, string developmentTeamId)
{
  _BuildXcodeArchive(
    projectPath: projectPath,
    archivePath: archivePath,
    derivedDataPath: derivedDataPath,
    developmentTeamId: developmentTeamId);

  _BuildXcodeIpa(
    archivePath: archivePath,
    exportPath: exportPath,
    exportOptionsPlistPath: exportOptionsPlistPath);
}

void _BuildXcodeArchive(string projectPath, string archivePath, string derivedDataPath,
  string developmentTeamId)
{
  var arguments =
    new ProcessArgumentBuilder()
      .Append("-project").AppendQuoted(projectPath)
      .Append("-scheme").Append("Unity-iPhone")
      .Append("-configuration").Append("Release").Append("clean").Append("archive")
      .Append("-verbose")
      .Append("-IDEBuildOperationMaxNumberOfConcurrentCompileTasks=16")
      .Append("-archivePath").AppendQuoted(archivePath)
      .Append("-derivedDataPath").AppendQuoted(derivedDataPath)
      .Append($"DEVELOPMENT_TEAM={developmentTeamId}")
      .Append("-allowProvisioningUpdates");

  Information($"Running Xcode archive build...");
  Information($"Arguments: {arguments.Render()}");

  StartProcess("xcodebuild", new ProcessSettings { Arguments = arguments });
}

void _BuildXcodeIpa(string archivePath, string exportPath, string exportOptionsPlistPath)
{
  var arguments =
    new ProcessArgumentBuilder()
      .Append("-exportArchive")
      .Append("-archivePath").AppendQuoted(archivePath)
      .Append("-exportOptionsPlist").AppendQuoted(exportOptionsPlistPath)
      .Append("-exportPath").AppendQuoted(exportPath);

  Information($"Running Xcode .ipa build...");
  Information($"Arguments: {arguments.Render()}");

  StartProcess("xcodebuild", new ProcessSettings { Arguments = arguments });
}*/
