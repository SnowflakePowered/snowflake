using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading.Tasks;
using System.Composition;
using Snowflake.Language.CodeActions;

namespace Snowflake.Language.Analyzers.Configuration
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class CannotHideInheritedPropertyFix
        : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticCodes.SFC004__CannotHideInheritedProperty);

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeSyntax = syntaxRoot?.FindNode(context.Span);
            if (nodeSyntax != null)
                context.RegisterCodeFix(new DeleteNodeAction(context.Document, nodeSyntax), context.Diagnostics);
        }
    }
}
