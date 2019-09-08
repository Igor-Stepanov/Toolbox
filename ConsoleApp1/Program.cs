using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AttributeExchange;
using Microsoft.CodeAnalysis.MSBuild;

namespace ConsoleApp1
{
  public static class VisualStudioProvider
  {
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
      var directory = new DirectoryInfo(currentPath ?? Directory.GetCurrentDirectory());
      while (directory != null && !directory.GetFiles("*.sln").Any())
        directory = directory.Parent;
      return directory;
    }
  }
  
  public class JsonAttribute : Attribute
  {}
  
  public class KeyAttribute : Attribute
  {}


  public class Simple
  {
    [Json]
    public int _field;
  }
  
  
  class Program
  {
    static void Main(string[] args)
    {
      var t1 = VisualStudioProvider.TryGetSolutionDirectoryInfo().GetFiles("*.sln").First().FullName;
      ModifySolutionUsingSyntaxRewriter(t1);
      //t.Wait();
      
      Debugger.Break();
    }
    
    private static void ModifySolutionUsingSyntaxRewriter(string solutionPath)
    {
      using (var workspace = MSBuildWorkspace.Create())
      {
        // Selects a Solution File
        var solutionTask = workspace.OpenSolutionAsync(solutionPath);
        var solution = solutionTask.Result;
        // Iterates through every project
        foreach (var project in solution.Projects)
        {
          // Iterates through every file
          foreach (var document in project.Documents)
          {
            // Selects the syntax tree
            var syntaxTree = document.GetSyntaxTreeAsync().Result;
            var root = syntaxTree.GetRoot();

            // Generates the syntax rewriter
            var rewriter = new AttributeStatementChanger();
            root = rewriter.Visit(root);

            // Exchanges the document in the solution by the newly generated document
            solution = solution.WithDocumentSyntaxRoot(document.Id, root);
          }
        }

        // applies the changes to the solution
        workspace.TryApplyChanges(solution);
      }
    }
  }
}