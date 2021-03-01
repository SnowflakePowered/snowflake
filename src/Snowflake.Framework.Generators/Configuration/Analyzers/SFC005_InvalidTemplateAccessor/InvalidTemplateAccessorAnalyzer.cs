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
    public sealed class InvalidTemplateAccessorAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC005__InvalidTemplateAccessor,
                title: "Template properties can not have accessor bodies",
                messageFormat: "Property '{0}' has an accessor body for '{1}' but must be auto-implemented",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Template properties must be auto-implemented and are not allowed to specify a body.");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node)
        {
            var interfaceSyntax = (InterfaceDeclarationSyntax)node;
            var types = new ConfigurationTypes(compilation);
            var interfaceSymbol = semanticModel.GetDeclaredSymbol(interfaceSyntax);
            if (interfaceSymbol == null)
                yield break;

            if (!interfaceSymbol.GetAttributes().Any(a =>
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationCollectionAttribute)
               || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationSectionAttribute)
               || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                yield break;

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                foreach (var member in childIface.GetMembers().Where(s => s.Kind == SymbolKind.Property).Cast<IPropertySymbol>())
                {
                    if (member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() 
                        is not PropertyDeclarationSyntax propertySyntax)
                        continue;

                    if (propertySyntax.ExpressionBody is ArrowExpressionClauseSyntax expressionGet)
                    {
                        yield return Diagnostic.Create(Rule, expressionGet.GetLocation(), member.Name, "get");
                        continue;
                    }

                    if (propertySyntax.AccessorList is AccessorListSyntax accessors)
                    {
                        foreach (var accessor in accessors.Accessors)
                        {
                            if (accessor.ExpressionBody != null || accessor.Body != null)
                                yield return Diagnostic.Create(Rule, accessor.GetLocation(), member.Name, accessor.Keyword.Text);
                        }
                    }
                }
            }
        }
    }
}
