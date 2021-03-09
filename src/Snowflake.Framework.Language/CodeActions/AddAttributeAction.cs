using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Snowflake.Language.CodeActions
{
    public abstract class AddAttributeAction
        : CodeAction
    {
        public AddAttributeAction(Document document, SyntaxNode declaration, string attributeName)
        {
            this.Document = document;
            this.Declaration = declaration;
            this.AttributeName = attributeName;
        }

        public override string Title => $"Add [{AttributeName}]";

        public Document Document { get; }
        public SyntaxNode Declaration { get; }
        public string AttributeName { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected abstract IEnumerable<ExpressionSyntax> GetArgumentList();

        protected sealed override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);

            var attributeArguments = this.GetArgumentList().Select(arg => SyntaxFactory.AttributeArgument(arg));
            editor.AddAttribute(this.Declaration, SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName(this.AttributeName),
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SeparatedList(attributeArguments))));
            return editor.GetChangedDocument();
        }
    }
}
