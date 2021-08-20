using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Snowflake.Language.Generators.Configuration;

namespace Snowflake.Language.Analyzers.Configuration
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class OnlyOneAttributeTemplateTypeAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC019__OnlyOneAttributeTemplateType,
                title: "Template interface can not be both an [InputConfiguration] and a [ConfigurationSection]",
                messageFormat: "Template interface '{0}' is decorated with both [ConfiguratiSection] and [InputConfiguration]",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax, cancel);
            if (interfaceSymbol == null)
                yield break;

            if (interfaceSymbol.GetAttributes().Any(a =>
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute))
                && interfaceSymbol.GetAttributes().Any(a =>
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                yield return Diagnostic.Create(Rule, interfaceSymbol.Locations.First(), interfaceSymbol.Name);
        }
    }
}
