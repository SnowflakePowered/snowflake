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

namespace Snowflake.Language.Analyzers
{
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

        private IEnumerable<Diagnostic> Analyze(INamedTypeSymbol marker, INamedTypeSymbol target, SemanticModel semanticModel, 
            InvocationExpressionSyntax invocationSyntax, CancellationToken cancel)
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

        private IEnumerable<Diagnostic> Analyze(INamedTypeSymbol marker, INamedTypeSymbol target, SemanticModel semanticModel,
            BaseObjectCreationExpressionSyntax newObjSyntax)
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

        private IEnumerable<Diagnostic> Analyze(INamedTypeSymbol marker, INamedTypeSymbol target, SemanticModel semanticModel,
            VariableDeclarationSyntax varDeclSyntax)
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

        private IEnumerable<Diagnostic> Analyze(INamedTypeSymbol marker, INamedTypeSymbol target, SemanticModel semanticModel,
            ParameterSyntax paramSyntax)
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

        public sealed override IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel)
        {
            (var marker, var target) = this.GetAttributes(compilation);

            return node switch
            {
                InvocationExpressionSyntax invocationSyntax => this.Analyze(marker, target, semanticModel, invocationSyntax, cancel),
                BaseObjectCreationExpressionSyntax newObjSyntax => this.Analyze(marker, target, semanticModel, newObjSyntax),
                VariableDeclarationSyntax varDeclSyntax => this.Analyze(marker, target, semanticModel, varDeclSyntax),
                ParameterSyntax paramSyntax => this.Analyze(marker, target, semanticModel, paramSyntax),
                _ => Enumerable.Empty<Diagnostic>()
            };
        }
    }
}
