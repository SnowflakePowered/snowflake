using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Snowflake.Language.CodeActions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Language.Analyzers.Extensibility
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class ComposerCallsUnimportedServiceFix
         : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticCodes.SFE003__ComposerCallsUnimportedService);

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeSyntax = syntaxRoot?.FindNode(context.Span);
            if (nodeSyntax == null)
                return;
            if (nodeSyntax is not MemberAccessExpressionSyntax memberSyntax)
                return;
            if (memberSyntax.FirstAncestorOrSelf<MethodDeclarationSyntax>() is not MethodDeclarationSyntax composeMethod)
                return;
            if (memberSyntax.DescendantNodes()
                .FirstOrDefault(a => a is GenericNameSyntax) is not GenericNameSyntax syntaxGenerics)
                return;
            if (syntaxGenerics.TypeArgumentList.Arguments.FirstOrDefault() is not TypeSyntax serviceType)
                return;
            context.RegisterCodeFix(new AddImportServiceAttributeAction(context.Document, 
                composeMethod, serviceType), context.Diagnostics);
        }
    }
}
