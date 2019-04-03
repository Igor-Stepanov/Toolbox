void TestCakeScript(string scriptPath)
{
  Information($"Verifying that '{scriptPath}' is valid Cake script...");

  CakeExecuteScript(scriptPath, new CakeSettings
  {
    Arguments = new Dictionary<string, string>{ { "-dryrun", "" } },
  });
}

void TestCakeScripts(IEnumerable<FilePath> scriptPathes) =>
  TestCakeScripts(scriptPathes.Select(scriptPath => scriptPath.FullPath));

void TestCakeScripts(IEnumerable<string> scriptPathes)
{
  foreach (var scriptPath in scriptPathes)
  {
    TestCakeScript(scriptPath);
  }
}
