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

namespace Snowflake.Generators.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public abstract class AbstractGenericTypeRequiresAttributeAnalyzer
        : AbstractSyntaxNodeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected sealed override IEnumerable<SyntaxKind> Kinds => new[] {
            SyntaxKind.InvocationExpression,
            SyntaxKind.ObjectCreationExpression,
            SyntaxKind.ImplicitObjectCreationExpression,
            SyntaxKind.VariableDeclaration,
            SyntaxKind.Parameter
        };

        protected abstract DiagnosticDescriptor Rule { get; }

        protected abstract (INamedTypeSymbol marker, INamedTypeSymbol target) GetAttributes(Compilation compilation);

        public sealed override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            (var marker, var target) = this.GetAttributes(compilation);
            if (node is InvocationExpressionSyntax invocationSyntax)
            {
                if (semanticModel.GetSymbolInfo(invocationSyntax, cancel).Symbol is not IMethodSymbol method)
                    yield break;

                if (!method.IsGenericMethod
                    || method.GetAttributes()
                        .FirstOrDefault(a =>
                            SymbolEqualityComparer
                            .Default.Equals(a.AttributeClass, marker)) is not AttributeData genericTypeAttribute)
                    yield break;
                if (genericTypeAttribute.ConstructorArguments.FirstOrDefault().Value is not int genericIndex)
                    yield break;
                if (method.TypeArguments.ElementAtOrDefault(genericIndex) is not INamedTypeSymbol targetType)
                    yield break;
                if (!targetType.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, target)))
                    yield return Diagnostic.Create(Rule, invocationSyntax.GetLocation(), targetType.Name);
            }

            if (node is BaseObjectCreationExpressionSyntax newObjSyntax)
            {
                // ImplicitObject has no type so instead we get the constructor, then the containing type of the constructor.
                if (semanticModel.GetSymbolInfo(newObjSyntax).Symbol?.ContainingType is not INamedTypeSymbol type)
                    yield break;
                if (!type.IsGenericType
                    || type.GetAttributes()
                        .FirstOrDefault(a =>
                            SymbolEqualityComparer
                            .Default.Equals(a.AttributeClass, marker)) is not AttributeData genericTypeAttribute)
                    yield break;
                if (genericTypeAttribute.ConstructorArguments.FirstOrDefault().Value is not int genericIndex)
                    yield break;
                if (type.TypeArguments.ElementAtOrDefault(genericIndex) is not INamedTypeSymbol targetType)
                    yield break;
                if (!targetType.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, target)))
                    yield return Diagnostic.Create(Rule, newObjSyntax.GetLocation(), targetType.Name);
            }

            if (node is VariableDeclarationSyntax varDeclSyntax)
            {
                if (semanticModel.GetSymbolInfo(varDeclSyntax.Type).Symbol is not INamedTypeSymbol type)
                    yield break;
                if (!type.IsGenericType
                   || type.GetAttributes()
                       .FirstOrDefault(a =>
                           SymbolEqualityComparer
                           .Default.Equals(a.AttributeClass, marker)) is not AttributeData genericTypeAttribute)
                    yield break;
                if (genericTypeAttribute.ConstructorArguments.FirstOrDefault().Value is not int genericIndex)
                        yield break;
                if (type.TypeArguments.ElementAtOrDefault(genericIndex) is not INamedTypeSymbol targetType)
                    yield break;
                if (!targetType.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, target)))
                    yield return Diagnostic.Create(Rule, varDeclSyntax.GetLocation(), targetType.Name);
            }

            if (node is ParameterSyntax paramSyntax)
            {
                if (paramSyntax.Type == null)
                    yield break;
                if (semanticModel.GetSymbolInfo(paramSyntax.Type).Symbol is not INamedTypeSymbol type)
                    yield break;
                if (!type.IsGenericType
                   || type.GetAttributes()
                       .FirstOrDefault(a =>
                           SymbolEqualityComparer
                           .Default.Equals(a.AttributeClass, marker)) is not AttributeData genericTypeAttribute)
                    yield break;
                if (genericTypeAttribute.ConstructorArguments.FirstOrDefault().Value is not int genericIndex)
                    yield break;
                if (type.TypeArguments.ElementAtOrDefault(genericIndex) is not INamedTypeSymbol targetType)
                    yield break;
                if (!targetType.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, target)))
                    yield return Diagnostic.Create(Rule, paramSyntax.GetLocation(), targetType.Name);
            }
        }
    }
}
