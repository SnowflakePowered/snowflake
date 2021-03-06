using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;
using System.Collections.Generic;

namespace Snowflake.Language.CodeActions
{
    public sealed class AddConfigurationSectionAttributeAction
        : AddAttributeAction
    {
        public AddConfigurationSectionAttributeAction(Document document, SyntaxNode declaration)
            : base(document, declaration, "ConfigurationSection")
        {
  
        }

        protected override IEnumerable<ExpressionSyntax> GetArgumentList()
        {
            yield return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("(CHANGE ME!) sectionName"));
            yield return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("(CHANGE ME!) displayName"));
        }
    }
}
