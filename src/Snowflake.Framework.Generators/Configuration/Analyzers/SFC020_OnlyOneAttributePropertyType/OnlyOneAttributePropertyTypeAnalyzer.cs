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
    public sealed class OnlyOneAttributePropertyTypeAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC020__OnlyOneAttributePropertyType,
                title: "InputConfiguration template property is can not be both a [ConfigurationOption] and [InputOption]",
                messageFormat: "Property '{0}' can only have one of [ConfigurationOption] or [InputOption]",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Properties in an InputConfiguation template can not be decorated with both [ConfigurationOption] and [InputOption].");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax);
            if (interfaceSymbol == null)
                yield break;

            if (!interfaceSymbol.GetAttributes().Any(a =>
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                yield break;

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                if (SymbolEqualityComparer.Default.Equals(childIface, types.IInputConfigurationTemplate))
                    continue;
                foreach (var member in childIface.GetMembers().Where(s => s.Kind == SymbolKind.Property).Cast<IPropertySymbol>())
                {
                    if (member.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationOptionAttribute))
                        && member.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputOptionAttribute)))
                    {
                        yield return Diagnostic.Create(Rule, member.Locations.First(), member.Name);
                    }
                }
            }
        }
    }
}
