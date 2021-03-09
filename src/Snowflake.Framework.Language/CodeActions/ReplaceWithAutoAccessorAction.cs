using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.Language.CodeActions
{
    public sealed class ReplaceWithAutoAccessorAction
        : CodeAction
    {
        public ReplaceWithAutoAccessorAction(Document document, SyntaxNode declaration)
        {
            this.Document = document;
            this.Node = declaration;
        }

        public override string Title => $"Replace with auto-implemented accessor";

        public Document Document { get; }
        public SyntaxNode Node { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);

            if (this.Node is ArrowExpressionClauseSyntax autoGet && autoGet.Parent is PropertyDeclarationSyntax propertyNode)
            {
                editor.ReplaceNode(propertyNode, propertyNode
                    .RemoveNode(autoGet, SyntaxRemoveOptions.KeepNoTrivia)!
                    .WithAccessorList(SyntaxFactory
                        .AccessorList(new SyntaxList<AccessorDeclarationSyntax>(
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))))
                        .WithoutTrivia())
                    .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.None))
                    .WithoutTrailingTrivia());
            }
            else if (this.Node is AccessorDeclarationSyntax accessor)
            {
                editor.ReplaceNode(this.Node, 
                       SyntaxFactory.AccessorDeclaration(accessor.Kind())
                       .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                       .WithoutTrivia()
                       );
            }           
            return editor.GetChangedDocument();
        }
    }
}
