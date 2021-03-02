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
    public sealed class DuplicateConfigurationTargetAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(EdgeRule, RootRule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor EdgeRule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC009__DuplicateConfigurationTarget,
                title: "A ConfigurationTarget with the same edge has already been declared.",
                messageFormat: "A ConfigurationTarget going from '{0}' to '{1}' has already been declared for ConfigurationCollection template '{2}'",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        private static readonly DiagnosticDescriptor RootRule =
         new DiagnosticDescriptor(
             id: DiagnosticCodes.SFC009__DuplicateConfigurationTarget,
             title: "A root ConfigurationTarget with the same name has already been declared",
             messageFormat: "A ConfigurationTarget root with the name '{0}' has already been declared for ConfigurationCollection template '{1}'",
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
               SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationCollectionAttribute)))
                yield break;


            var targetAttrs = new List<AttributeData>();

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                targetAttrs.AddRange(childIface.GetAttributes()
                    .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetAttribute)));
            }

            var rootTargets = new HashSet<string>();
            var childTargets = new HashSet<(string, string)>();
            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length == 1)
                {
                    string rootTarget = (string)targetAttr.ConstructorArguments[0].Value!;
                    if (!rootTargets.Contains(rootTarget))
                    {
                        rootTargets.Add(rootTarget);
                    }
                    else
                    {
                        yield return Diagnostic.Create(RootRule,
                            targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                            rootTarget,
                            interfaceSymbol.Name
                            );
                    }
                }
                else if (targetAttr.ConstructorArguments.Length == 2)
                {
                    (string childTarget, string parentTarget) = ((string)targetAttr.ConstructorArguments[0].Value!,
                        (string)targetAttr.ConstructorArguments[1].Value!);
                    if (!childTargets.Contains((parentTarget, childTarget)))
                    {
                        childTargets.Add((parentTarget, childTarget));
                    }
                    else
                    {
                        yield return Diagnostic.Create(EdgeRule,
                           targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                           parentTarget, childTarget, interfaceSymbol.Name);
                    }
                }
            }
        }
    }
}
