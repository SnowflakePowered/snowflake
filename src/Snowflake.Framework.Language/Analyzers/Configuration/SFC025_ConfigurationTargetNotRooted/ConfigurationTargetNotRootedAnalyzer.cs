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
    public sealed class ConfigurationTargetNotRootedAnalyzers
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.InterfaceDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC025__ConfigurationTargetNotRooted,
                title: "The specified ConfigurationTarget is not rooted.",
                messageFormat: "The configuration target named '{0}' is not attached to a root target.",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: "The specified ConfigurationTarget does not have a root and will not be traversed.");

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
            }

            // Collect declared root targets
            var rootTargets = new HashSet<string>();
            var adjacency = new Dictionary<string, HashSet<string>>();
            var sortedTargets = new List<string>();
            var leaves = new Queue<string>();
            // null target always exists.
            rootTargets.Add("#null");

            // Build root targets
            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length == 1)
                {
                    string rootTarget = (string)targetAttr.ConstructorArguments[0].Value!;
                    if (!rootTargets.Contains(rootTarget))
                    {
                        rootTargets.Add(rootTarget);
                    }
                }
            }

            // build adjacency list
            foreach (var rootTarget in rootTargets)
            {
                adjacency[rootTarget] = new HashSet<string>();
            }

            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length == 2)
                {
                    (string childTarget, string parentTarget) = ((string)targetAttr.ConstructorArguments[0].Value!,
                     (string)targetAttr.ConstructorArguments[1].Value!);

                    if (!adjacency.ContainsKey(childTarget))
                        adjacency[childTarget] = new HashSet<string>();
                    if (!adjacency.ContainsKey(parentTarget))
                        adjacency[parentTarget] = new HashSet<string>();

                    // Edges are directed from parents to children.
                    adjacency[parentTarget].Add(childTarget);

                }
            }

            // finding reachable nodes from rootTargets

            var reachable = new HashSet<string>(rootTargets);
            var toProcess = new Queue<string>(rootTargets);
            while (toProcess.Any())
            {
                var targetNode = toProcess.Dequeue();
                foreach (var childNodes in adjacency[targetNode])
                {
                    reachable.Add(childNodes);
                    toProcess.Enqueue(childNodes);
                }
            }

            foreach (var targetAttr in targetAttrs)
            {
                if (targetAttr.ConstructorArguments.Length == 2)
                {
                    (string childTarget, string parentTarget) = ((string)targetAttr.ConstructorArguments[0].Value!,
                     (string)targetAttr.ConstructorArguments[1].Value!);
                    if (!reachable.Contains(childTarget))
                    {
                        yield return Diagnostic.Create(Rule,
                         targetAttr.ApplicationSyntaxReference?.GetSyntax().GetLocation(),
                         childTarget, interfaceSymbol.Name);
                    }                    
                }
            }
        }
    }
}
