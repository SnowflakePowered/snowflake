using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Snowflake.Language.Generators.Configuration;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Snowflake.Language.Analyzers.Extensibility
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class PluginMustHavePluginAttributeAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.ClassDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFE001__PluginMustHavePluginAttribute,
                title: "Plugin must be marked with [Plugin].",
                messageFormat: "Concrete class '{0}' implements 'IPlugin' but does not have [Plugin] attribute.",
                category: "Snowflake.Extensibility",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Concrete implementations of IPlugin must be marked with [Plugin]");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            var classSyntax = (ClassDeclarationSyntax)node;
            var types = new ExtensibilityTypes(compilation);
            var classSymbol = semanticModel.GetDeclaredSymbol(classSyntax, cancel);
            if (classSymbol == null)
                yield break;
            // only look at concrete implementations.
            if (classSymbol.IsAbstract)
                yield break;
            if (!classSymbol.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, types.IPlugin)))
                yield break;
            if (!classSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.PluginAttribute)))
            {
                yield return Diagnostic.Create(Rule, classSyntax.GetLocation(), classSymbol.Name);
            }
        }
    }
}
