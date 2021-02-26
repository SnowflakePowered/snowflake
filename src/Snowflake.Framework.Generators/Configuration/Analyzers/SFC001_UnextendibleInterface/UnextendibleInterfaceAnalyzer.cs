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

namespace Snowflake.Generators.Configuration.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class UnextendibleInterfaceAnalyzer
        : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        private static readonly DiagnosticDescriptor Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC001__UnextendibleInterfaceAnalyzer,
                title: "Template interface is not extendible",
                messageFormat: "Interface '{0}' must be partial so it can implement generated proxy support members", 
                category: "Configuration", 
                DiagnosticSeverity.Error, 
                isEnabledByDefault: true, 
                customTags: new [] { WellKnownDiagnosticTags.NotConfigurable },
                description: "Template interface must be marked as partial.");

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(this.AnalyzeNode, SyntaxKind.InterfaceDeclaration);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var types = new ConfigurationTypes(context.Compilation);
            var interfaceSyntax = (InterfaceDeclarationSyntax)context.Node;
            var interfaceSymbol = context.SemanticModel.GetDeclaredSymbol(interfaceSyntax);
            if (interfaceSymbol == null)
                return;
            if (!interfaceSymbol.GetAttributes().Any(a =>
                SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.ConfigurationCollectionAttribute)
                || SymbolEqualityComparer.Default.Equals(a.AttributeClass, types.InputConfigurationAttribute)))
                return;
            if (!interfaceSyntax.Modifiers.Any(p => p.IsKind(SyntaxKind.PartialKeyword)))
            {
                var diagnostic = Diagnostic.Create(Rule, interfaceSyntax.GetLocation(), interfaceSymbol.Name);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
