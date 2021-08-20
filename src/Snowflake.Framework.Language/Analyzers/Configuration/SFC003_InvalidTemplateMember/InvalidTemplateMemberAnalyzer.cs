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
    public sealed class InvalidTemplateMemberAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC003__InvalidTemplateMemberAnalyzer,
                title: "Invalid members in template interface",
                messageFormat: "{0} '{1}.{2}' is not a non-indexer property",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Template interface must only declare non-indexer property members.");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(node, cancel) as INamedTypeSymbol;
            if (interfaceSymbol == null)
                yield break;

            if (!interfaceSymbol.GetAttributes().Any(a =>
                SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationCollectionAttribute)
                || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)
                || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)))
                yield break;

            foreach (var member in interfaceSymbol.GetMembers())
            {
                // Ignore accessor implementations for this diagnostic.
                if (member is IMethodSymbol accessor && accessor.AssociatedSymbol is IPropertySymbol)
                    continue;
                if (member is not IPropertySymbol propertySymbol 
                    || propertySymbol.IsIndexer)
                {
                    yield return Diagnostic.Create(Rule, member.Locations.FirstOrDefault(), member.Kind, interfaceSymbol.Name, member.Name);
                }
            }
        }
    }
}
