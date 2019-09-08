using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AttributeExchange
{
  /// The CSharpSyntaxRewriter allows to rewrite the Syntax of a node
  /// </summary>
  public class AttributeStatementChanger : CSharpSyntaxRewriter
  {
    /// Visited for all AttributeListSyntax nodes
    /// The method replaces all PreviousAttribute attributes annotating a method by ReplacementAttribute attributes
    public override SyntaxNode VisitAttributeList(AttributeListSyntax node)
    {
      if (node.Parent is FieldDeclarationSyntax && node.Attributes.Any(x => x.Name.GetText().ToString() == "Json"))
        return SyntaxFactory.AttributeList(
          SyntaxFactory.SingletonSeparatedList(
            SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("Key"),
              SyntaxFactory.AttributeArgumentList(
                SyntaxFactory.SeparatedList(new[]
                {
                  SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(@"Sample"))
                  )
                })))));
      
      // Otherwise the node is left untouched
      return base.VisitAttributeList(node);
    }
  }
}