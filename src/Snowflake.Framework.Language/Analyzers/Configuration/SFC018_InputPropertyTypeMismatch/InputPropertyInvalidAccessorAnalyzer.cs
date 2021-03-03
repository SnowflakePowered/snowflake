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
    public sealed class InputPropertyTypeMismatchAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC018__InputPropertyTypeMismatch,
                title: "InputConfiguration input template properties must be of type DeviceCapability.",
                messageFormat: "Property '{0}' is of type '{1}' but must be of type DeviceCapability",
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

            if (!interfaceSymbol.GetAttributes().Any(a =>
                SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                yield break;

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                if (SymbolEqualityComparer.Default.Equals(childIface, types.IInputConfigurationTemplate))
                    continue;
                foreach (var member in childIface.GetMembers().Where(s => s.Kind == SymbolKind.Property).Cast<IPropertySymbol>())
                {
                    if (member.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax() 
                        is not PropertyDeclarationSyntax propertySyntax)
                        continue;
                    if (!member.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputOptionAttribute)))
                        continue;
                    if (!SymbolEqualityComparer.Default.Equals(member.Type, types.DeviceCapability))
                        yield return Diagnostic.Create(Rule, member.Locations.FirstOrDefault(), member.Name, member.Type);
                }
            }
        }
    }
}
