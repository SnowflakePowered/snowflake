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
    public sealed class ConfigurationTargetDoesNotExistAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC024__ConfigurationTargetDoesNotExist,
                title: "A ConfigurationTarget with the specified name does not exist",
                messageFormat: "A configuration target named '{0}' was not found",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: "ConfigurationTargetMembers and child ConfigurationTargets that refer to non-existent root targets will have no effect.");

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
            var props = new List<ISymbol>();

            foreach (var childIface in interfaceSymbol.AllInterfaces.Reverse().Concat(new[] { interfaceSymbol }))
            {
                targetAttrs.AddRange(childIface.GetAttributes()
                    .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetAttribute)));

                props.AddRange(childIface.GetMembers().Where(m => m.GetAttributes()
                    .Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetMemberAttribute))));
            }

            // Collect pass
            var targetNames = new HashSet<string>();

            // null target is always valid.
            targetNames.Add("#null");

            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length >= 1)
                {
                    string targetName = (string)targetAttr.ConstructorArguments[0].Value!;
                    if (!targetNames.Contains(targetName))
                    {
                        targetNames.Add(targetName);
                    }
                }
            }

            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length != 2)
                    continue;

                string parentName = (string)targetAttr.ConstructorArguments[1].Value!;
                if (!targetNames.Contains(parentName))
                {
                    yield return Diagnostic.Create(Rule,
                                           targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                                           parentName);
                }
            }

            foreach (var prop in props)
            {
                var memberEntries = prop.GetAttributes()
                    .Where(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationTargetMemberAttribute));
                foreach (var entry in memberEntries)
                {
                    if (entry.ConstructorArguments.Length < 1)
                        continue;
                    string targetName = (string)entry.ConstructorArguments[0].Value!;

                    if (!targetNames.Contains(targetName))
                    {
                        yield return Diagnostic.Create(Rule,
                                           entry.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                                           targetName);
                    }
                }
            }
        }
    }
}
