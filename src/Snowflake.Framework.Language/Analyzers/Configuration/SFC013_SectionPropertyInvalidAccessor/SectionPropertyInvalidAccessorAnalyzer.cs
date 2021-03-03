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
    public sealed class SectionPropertyInvalidAccessorAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC013__SectionPropertyInvalidAccessor,
                title: "ConfigurationSection template properties can only have 'get' and 'set' accessors.",
                messageFormat: "Declared accessor '{1}' is invalid for property '{0}'",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "ConfigurationSection template properties must only declare 'get' and 'set' accessors.");

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

                    if (propertySyntax.AccessorList is AccessorListSyntax accessors)
                    {
                        foreach (var accessor in accessors.Accessors)
                        {
                            if (!accessor.IsKind(SyntaxKind.GetAccessorDeclaration) && !accessor.IsKind(SyntaxKind.SetAccessorDeclaration))
                                yield return Diagnostic.Create(Rule, accessor.GetLocation(), member.Name, accessor.Keyword.Text);
                        }
                    }
                }
            }
        }
    }
}
