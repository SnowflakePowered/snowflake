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
using System.Threading;

namespace Snowflake.Language.Analyzers.Configuration
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ConfigurationTargetAlreadyExistsAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
         new DiagnosticDescriptor(
             id: DiagnosticCodes.SFC009__DuplicateConfigurationTarget,
             title: "A ConfigurationTarget with the same name has already been declared",
             messageFormat: "A ConfigurationTarget with the name '{0}' has already been declared for ConfigurationCollection template '{1}'",
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

            var declaredTargets = new HashSet<string>();

            // null target always exists.
            declaredTargets.Add("#null");

            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length > 0)
                {
                    string rootTarget = (string)targetAttr.ConstructorArguments[0].Value!;
                    if (!declaredTargets.Contains(rootTarget))
                    {
                        declaredTargets.Add(rootTarget);
                    }
                    else
                    {
                        yield return Diagnostic.Create(Rule,
                            targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                            rootTarget,
                            interfaceSymbol.Name
                            );
                    }
                }
            }
        }
    }
}
