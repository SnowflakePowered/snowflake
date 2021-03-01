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
    public sealed class SectionPropertyEnumUndecoratedAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC012__SectionPropertyEnumUndecorated,
                title: "ConfigurationSection template property undecorated selection option enum member",
                messageFormat: "Template property '{0}' has enum type {1} but not all enum members are decorated with [SelectionOption]",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Enums used in ConfigurationSection template properties must have every member decorated with [SelectionOption].");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax);
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
                    if (member.GetAttributes()
                        .FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationOptionAttribute)) is not AttributeData)
                        continue;
                    if (member.Type.TypeKind != TypeKind.Enum)
                        continue;
                    if (!member.Type.GetMembers().Where(e => e.Kind == SymbolKind.Field)
                        .All(e => e.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.SelectionOptionAttribute))))
                    {
                        var propertySyntax = member.DeclaringSyntaxReferences.First();
                        if (propertySyntax.GetSyntax().GetDiagnostics().Any(d => d.Id == DiagnosticCodes.SFC012__SectionPropertyEnumUndecorated))
                            continue;
                        yield return Diagnostic.Create(Rule, member.Locations.FirstOrDefault(),
                                   member.Name, member.Type);
                    }
                }
            }
        }
    }
}
