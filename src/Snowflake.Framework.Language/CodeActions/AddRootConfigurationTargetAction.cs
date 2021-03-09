using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.CodeActions
{
    public sealed class AddRootConfigurationTargetAction
        : AddAttributeAction
    {
        public AddRootConfigurationTargetAction(Document document, SyntaxNode declaration, string targetName) 
            : base(document, declaration, "ConfigurationTarget")
        {
            this.TargetName = targetName;
        }

        public string TargetName { get; }

        protected override IEnumerable<ExpressionSyntax> GetArgumentList()
        {
            yield return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(this.TargetName));
        }
    }
}
