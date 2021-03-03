using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Snowflake.Language.Generators.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Snowflake.Language;
using Snowflake.Language.Analyzers;
using System.Threading;

namespace Snowflake.Language.Analyzers.Configuration
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SectionPropertyMustHaveGetterAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(SectionRule, InputRule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor SectionRule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC014__CollectionPropertyMustHaveGetter,
                title: "ConfigurationSection template properties must declare a public 'get' accessor.",
                messageFormat: "Property '{0}' does not declare a 'get' accessor",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "ConfigurationSection template properties must declare a public 'get' accessor.");

        private static readonly DiagnosticDescriptor InputRule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC014__CollectionPropertyMustHaveGetter,
                title: "InputConfiguration option template properties must declare a public 'get' accessor",
                messageFormat: "Property '{0}' does not declare a 'get' accessor",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "InputConfiguration option template properties must declare a public 'get' accessor.");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax, cancel);
            if (interfaceSymbol == null)
                yield break;

            if (!interfaceSymbol.GetAttributes().Any(a =>
                SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)
                || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                yield break;

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                foreach (var member in childIface.GetMembers().Where(s => s.Kind == SymbolKind.Property).Cast<IPropertySymbol>())
                {
                    if (member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax()
                        is not PropertyDeclarationSyntax propertySyntax)
                        continue;

                    if (!member.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationOptionAttribute)))
                        continue;

                    if (propertySyntax.AccessorList is not AccessorListSyntax accessors 
                        || accessors.Accessors.FirstOrDefault(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) is not AccessorDeclarationSyntax getAccessor
                        || getAccessor.Modifiers.Any())
                    {
                        if (interfaceSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                            yield return Diagnostic.Create(SectionRule, propertySyntax.GetLocation(), member.Name);
                        if (interfaceSymbol.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputOptionAttribute)))
                            yield return Diagnostic.Create(InputRule, propertySyntax.GetLocation(), member.Name);
                    }
                }
            }
        }
    }
}
