using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.Language.CodeActions
{
    public sealed class AddAccessorAction
        : CodeAction
    {
        public AddAccessorAction(Document document, SyntaxNode declaration, SyntaxKind accessor)
        {
            this.Document = document;
            this.Node = declaration;
            this.Accessor = accessor;
        }

        public string GetAccessorString()
        {
            return this.Accessor switch
            {
                SyntaxKind.GetAccessorDeclaration => "get",
                SyntaxKind.SetAccessorDeclaration => "set",
                SyntaxKind.InitAccessorDeclaration => "init",
                _ => "BROKENANALYZER"
            };
        }
        public override string Title => $"Add '{this.GetAccessorString()}' accessor";

        public Document Document { get; }
        public SyntaxNode Node { get; }
        public SyntaxKind Accessor { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            if (this.Node is not PropertyDeclarationSyntax propertyNode)
                return this.Document;
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);
            var oldAccessorList = propertyNode.AccessorList ?? SyntaxFactory.AccessorList();

            editor.ReplaceNode(this.Node, propertyNode
                .WithAccessorList(oldAccessorList
                    .AddAccessors(SyntaxFactory.AccessorDeclaration(this.Accessor)
                        .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)))));
            return editor.GetChangedDocument();
        }
    }
}
