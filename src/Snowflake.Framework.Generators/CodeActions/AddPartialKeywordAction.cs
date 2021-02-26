using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;

namespace Snowflake.Generators.CodeActions
{
    public sealed class AddPartialKeywordAction
        : CodeAction
    {
        public AddPartialKeywordAction(Document document, SyntaxNode declaration)
        {
            this.Document = document;
            this.Declaration = declaration;
        }

        public override string Title => "Add 'partial' keyword to declaration.";

        public Document Document { get; }
        public SyntaxNode Declaration { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);
            editor.SetModifiers(this.Declaration, DeclarationModifiers.Partial);
            return editor.GetChangedDocument();
        }
    }
}
