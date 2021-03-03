using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Threading.Tasks;
using System.Composition;
using Snowflake.Language.CodeActions;
using System;

namespace Snowflake.Language.Analyzers.Configuration
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class CollectionPropertyNotSectionFix
        : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticCodes.SFC006__CollectionPropertyNotSection);

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeSyntax = syntaxRoot?.FindNode(context.Span);
            if (nodeSyntax == null)
                return;
            var model = await context.Document.GetSemanticModelAsync();
            if (model?.GetDeclaredSymbol(nodeSyntax) is not IPropertySymbol property)
                return;
            var typeReference = property.Type.DeclaringSyntaxReferences.FirstOrDefault();
            if (typeReference is not null && await typeReference.GetSyntaxAsync() is InterfaceDeclarationSyntax typeSyntax)
                context.RegisterCodeFix(new AddConfigurationSectionAttribute(context.Document, typeSyntax), context.Diagnostics);
        }
    }
}
