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
    public sealed class PluginMustNotBeNamedCommonAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.ClassDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFE002__PluginMustNotBeNamedCommon,
                title: "Plugin can not be named 'common'",
                messageFormat: "The class '{0}' exports a plugin with the reserved name 'common'",
                category: "Snowflake.Extensibility",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Plugins can not be named 'common' or they will conflict with the common resource directory.");

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
            if (classSymbol.GetAttributes()
                .FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.PluginAttribute)) is not AttributeData pluginAttribute)
                yield break;
            if (pluginAttribute.ConstructorArguments.FirstOrDefault().Value is not string attributeName)
                yield break;
            if (string.Equals(attributeName, "common", System.StringComparison.InvariantCultureIgnoreCase))
            {
                yield return Diagnostic.Create(Rule, pluginAttribute.ApplicationSyntaxReference?.GetSyntax(cancel).GetLocation(), classSymbol.Name);
            }
        }
    }
}
