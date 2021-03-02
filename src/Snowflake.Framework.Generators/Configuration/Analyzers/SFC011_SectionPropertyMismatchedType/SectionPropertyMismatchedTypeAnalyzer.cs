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
using System.Threading;

namespace Snowflake.Generators.Configuration.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class SectionPropertyMismatchedTypeAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC011__SectionPropertyMismatchedType,
                title: "ConfigurationSection template property type mismatch",
                messageFormat: "Property '{0}' has type '{1}' but is assigned a default value of type '{2}'",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "The type of the template property does not match with the type of the provided default value.");

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
                    if (member.GetAttributes()
                        .FirstOrDefault(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationOptionAttribute)) is not AttributeData attribute)
                        continue;
                    // If attribute has a second arg, then it must not be GUID-based 
                    if (attribute.ConstructorArguments.Skip(1).FirstOrDefault().Type is ITypeSymbol defaultType)
                    {
                        if (!SymbolEqualityComparer.Default.Equals(defaultType, member.Type))
                        {
                            yield return Diagnostic.Create(Rule,
                                member.Locations.FirstOrDefault(),
                                member.Name, member.Type, defaultType);
                        }
                    }
                    else if(attribute.ConstructorArguments.Length == 1 && !SymbolEqualityComparer.Default.Equals(member.Type, types.System_Guid))
                    {
                        yield return Diagnostic.Create(Rule,
                                member.Locations.FirstOrDefault(),
                                member.Name, member.Type, types.System_Guid);
                    }
                }
            }
        }
    }
}
