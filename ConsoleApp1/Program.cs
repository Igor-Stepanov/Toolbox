using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AttributeExchange;
using Microsoft.CodeAnalysis.MSBuild;

namespace ConsoleApp1
{
  public class JsonAttribute : Attribute
  {}
  
  public class KeyAttribute : Attribute
  {}


  public class Simple
  {
    [Json]
    public int _field;
  }

  public static class DirectoryInfoExtensions
  {
    public static IEnumerable<DirectoryInfo> Kek(this DirectoryInfo self)
    {
      if (self == null)
        yield break;
      
      do
      {
        yield return self;
        self = self.Parent;
      } 
      while (self != null);
    }
  }
  
  class Program
  {
    static void Main(string[] args)
    {
      var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
      while (directory != null && !directory.GetFiles("*.sln").Any())
        directory = directory.Parent;
      var t1 = directory.GetFiles("*.sln").First().FullName;
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