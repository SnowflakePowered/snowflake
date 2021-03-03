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
    public sealed class UnextendibleInterfaceFix
        : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticCodes.SFC001__UnextendibleInterfaceAnalyzer);

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var interfaceSyntax = syntaxRoot?.FindNode(context.Span);
            if (interfaceSyntax != null)
                context.RegisterCodeFix(new AddPartialKeywordAction(context.Document, interfaceSyntax), context.Diagnostics);
        }
    }
}
