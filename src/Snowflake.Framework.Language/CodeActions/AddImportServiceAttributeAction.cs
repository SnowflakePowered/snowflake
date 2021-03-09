using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.CodeActions
{
    public sealed class AddImportServiceAttributeAction
       : AddAttributeAction
    {
        public AddImportServiceAttributeAction(Document document, SyntaxNode node, TypeSyntax type)
            : base(document, node, "ImportService")
        {
            this.Service = type;
        }

        public override string Title => $"Add [ImportService(typeof({this.Service}))]";
        public override string EquivalenceKey => "Snowflake.Framework:Add [ImportService]";
        public TypeSyntax Service { get; }

        protected override IEnumerable<ExpressionSyntax> GetArgumentList()
        {
            yield return SyntaxFactory.TypeOfExpression(this.Service);
        }
    }
}
