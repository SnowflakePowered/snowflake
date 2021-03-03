using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using System.Threading;
using Microsoft.CodeAnalysis.Editing;

namespace Snowflake.Language.CodeActions
{
    public sealed class AddConfigurationSectionAttribute
        : CodeAction
    {
        public AddConfigurationSectionAttribute(Document document, SyntaxNode declaration)
        {
            this.Document = document;
            this.Declaration = declaration;
        }

        public override string Title => "Add [ConfigurationSection]";

        public Document Document { get; }
        public SyntaxNode Declaration { get; }

        public override string? EquivalenceKey => $"Snowflake.Framework:{this.Title}";

        protected override async Task<Document> GetChangedDocumentAsync(CancellationToken cancellationToken)
        {
            var editor = await DocumentEditor.CreateAsync(this.Document, cancellationToken).ConfigureAwait(false);
        
            editor.AddAttribute(this.Declaration, SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName("ConfigurationSection"),
                SyntaxFactory.AttributeArgumentList(
                    SyntaxFactory.SeparatedList(new[] {
                        SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("(CHANGE ME!) sectionName"))),
                        SyntaxFactory.AttributeArgument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("(CHANGE ME!) displayName")))
                        }
                    ))));
            return editor.GetChangedDocument();
        }
    }
}
