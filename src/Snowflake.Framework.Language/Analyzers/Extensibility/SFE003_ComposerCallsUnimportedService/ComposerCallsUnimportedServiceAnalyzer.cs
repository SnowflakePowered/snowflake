using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Snowflake.Language.Generators.Configuration;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Snowflake.Language.Analyzers.Extensibility
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class ComposerCallsUnimportedServiceAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override IEnumerable<SyntaxKind> Kinds => new[] { SyntaxKind.MethodDeclaration };

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFE003__ComposerCallsUnimportedService,
                title: "'Compose' requests unimported service",
                messageFormat: "Tried to get service '{0}' but an '[ImportService(typeof({0}))]' attribute was not found",
                category: "Snowflake.Extensibility",
                DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: "Services must be imported using [ImportService] before being available.");

        public override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            var types = new ExtensibilityTypes(compilation);
            var methodSyntax = (MethodDeclarationSyntax)node;
            if (semanticModel.GetDeclaredSymbol(methodSyntax) is not IMethodSymbol methodSymbol)
                yield break;
            if (methodSymbol.Name != "Compose")
                yield break;
            if (!methodSymbol.ContainingType.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, types.IComposable)))
                yield break;

            var importedServices = new HashSet<INamedTypeSymbol>();
           
            foreach (var attr in methodSymbol.GetAttributes())
            {
                if (!SymbolEqualityComparer.Default.Equals(attr.AttributeClass, types.ImportServiceAttribute))
                    continue;
                if (attr.ConstructorArguments.FirstOrDefault().Value is not INamedTypeSymbol importedType)
                    continue;
                importedServices.Add(importedType);
            }

            foreach (var expr in methodSyntax.Body?.DescendantNodes()
                .Where(a => a is MemberAccessExpressionSyntax).Cast<MemberAccessExpressionSyntax>() 
                ?? Enumerable.Empty<MemberAccessExpressionSyntax>())
            {
                if (semanticModel.GetSymbolInfo(expr, cancel).Symbol is not IMethodSymbol getMethod)
                    continue;
                if (getMethod.Name != "Get" || !getMethod.IsGenericMethod || !SymbolEqualityComparer.Default.Equals(getMethod.ContainingType, types.IServiceRepository))
                    continue;
                var requestedType = getMethod.TypeArguments.FirstOrDefault();
                if (!importedServices.Contains(requestedType))
                    yield return Diagnostic.Create(Rule, expr.GetLocation(), requestedType?.Name);
            }
        }
    }
}
