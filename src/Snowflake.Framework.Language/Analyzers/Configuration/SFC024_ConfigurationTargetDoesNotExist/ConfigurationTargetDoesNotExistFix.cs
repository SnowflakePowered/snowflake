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

namespace Snowflake.Language.Analyzers.Configuration
{
    [ExportCodeFixProvider(LanguageNames.CSharp), Shared]
    public sealed class ConfigurationTargetDoesNotExistFix
         : CodeFixProvider
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DiagnosticCodes.SFC024__ConfigurationTargetDoesNotExist);

        public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var syntaxRoot = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            var nodeSyntax = syntaxRoot?.FindNode(context.Span);

            if (nodeSyntax is not AttributeSyntax attributeSyntax)
                return;
            if (nodeSyntax.FirstAncestorOrSelf<InterfaceDeclarationSyntax>() is not InterfaceDeclarationSyntax interfaceSyntax)
                return;
            var model = await context.Document.GetSemanticModelAsync(context.CancellationToken);
            if (model == null)
                return;
            var attributeName = attributeSyntax.Name.ToString();
            if (attributeName.EndsWith("ConfigurationTargetMember")
                && attributeSyntax.ArgumentList?.Arguments.FirstOrDefault() is AttributeArgumentSyntax memberArgumentSyntax
                && memberArgumentSyntax.Expression is LiteralExpressionSyntax memberText)
            {
                context.RegisterCodeFix(new AddRootConfigurationTargetAction(context.Document,
                    interfaceSyntax, memberText.Token.ValueText), context.Diagnostics);
            } 
            else if (attributeName.EndsWith("ConfigurationTarget") 
                && attributeSyntax.ArgumentList?.Arguments.Count() == 2
                && attributeSyntax.ArgumentList?.Arguments.Skip(1).FirstOrDefault() is AttributeArgumentSyntax targetArgumentSyntax
                && targetArgumentSyntax.Expression is LiteralExpressionSyntax targetText)
            {
                context.RegisterCodeFix(new AddRootConfigurationTargetAction(context.Document,
                   interfaceSyntax, targetText.Token.ValueText), context.Diagnostics);
            }
           
        }
    }
}
