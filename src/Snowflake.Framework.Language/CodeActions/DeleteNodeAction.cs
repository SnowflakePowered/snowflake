using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;

namespace Snowflake.Language.CodeActions
{
    public sealed class DeleteNodeAction
        : CodeAction
    {
        public DeleteNodeAction(Document document, SyntaxNode declaration)
        {
            this.Document = document;
            this.Node = declaration;
        }

        public override string Title => "Remove the invalid member";

        public Document Document { get; }
        public SyntaxNode Node { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);
            editor.RemoveNode(this.Node);
            return editor.GetChangedDocument();
        }
    }
}
