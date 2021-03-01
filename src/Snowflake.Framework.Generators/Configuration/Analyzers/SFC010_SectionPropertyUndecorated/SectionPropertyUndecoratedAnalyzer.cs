using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Snowflake.Configuration.Generators;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Snowflake.Generators;
using Snowflake.Generators.Analyzers;

namespace Snowflake.Generators.Configuration.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SectionPropertyUndecoratedAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC010__SectionPropertyUndecorated,
                title: "ConfigurationSection template property is undecorated",
                messageFormat: "Property '{0}' must be decorated with [ConfigurationOption]",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "All properties in a ConfigurationSection template must be decorated with [ConfigurationOption].");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax);
            if (interfaceSymbol == null)
                yield break;

            if (!interfaceSymbol.GetAttributes().Any(a =>
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                yield break;

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                foreach (var member in childIface.GetMembers().Where(s => s.Kind == SymbolKind.Property).Cast<IPropertySymbol>())
                {
                    if (!member.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationOptionAttribute)))
                    {
                        yield return Diagnostic.Create(Rule, member.Locations.First(), member.Name);
                    }
                }
            }
        }
    }
}
