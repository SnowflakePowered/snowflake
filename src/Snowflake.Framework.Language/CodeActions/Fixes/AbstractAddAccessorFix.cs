using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading.Tasks;
using System.Composition;
using Snowflake.Language.CodeActions;
using Microsoft.CodeAnalysis.CSharp;

namespace Snowflake.Language.CodeActions.Fixes
{
    public abstract class AbstractAddAccessorFix
        : CodeFixProvider
    {
        protected AbstractAddAccessorFix(SyntaxKind accessor)
        {
            this.Accessor = accessor;
        }

        public SyntaxKind Accessor { get; }

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeSyntax = syntaxRoot?.FindNode(context.Span);
            if (nodeSyntax != null)
                context.RegisterCodeFix(new AddAccessorAction(context.Document, nodeSyntax, this.Accessor), context.Diagnostics);
        }
    }
}
