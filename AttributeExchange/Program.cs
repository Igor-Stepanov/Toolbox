using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Workspaces;

namespace AttributeExchange
{
  internal class Program
  {
    public static void Main(string[] args)
    {
    }
    
    /// The method calling the Syntax Rewriter
    private static async Task<bool> ModifySolutionUsingSyntaxRewriter(string solutionPath)
    {
      using (var workspace = MSBuildWorkspace.Create())
      {
        // Selects a Solution File
        var solution = await workspace.OpenSolutionAsync(solutionPath);
        // Iterates through every project
        foreach (var project in solution.Projects)
        {
          // Iterates through every file
          foreach (var document in project.Documents)
          {
            // Selects the syntax tree
            var syntaxTree = await document.GetSyntaxTreeAsync();
            var root = syntaxTree.GetRoot();

            // Generates the syntax rewriter
            var rewriter = new AttributeStatementChanger();
            root = rewriter.Visit(root);

            // Exchanges the document in the solution by the newly generated document
            solution = solution.WithDocumentSyntaxRoot(document.Id, root);
          }
        }

        // applies the changes to the solution
        var result = workspace.TryApplyChanges(solution);
        return result;
      }
    }
  }
}