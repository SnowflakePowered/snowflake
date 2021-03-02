using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;

namespace Snowflake.Generators.Analyzers
{
    public abstract class AbstractSyntaxNodeAnalyzer
        : DiagnosticAnalyzer
    {
        protected abstract IEnumerable<SyntaxKind> Kinds { get; }
        public abstract IEnumerable<Diagnostic> Analyze(Compilation compilation, SemanticModel semanticModel, SyntaxNode node, CancellationToken cancel = default);
        public sealed override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(this.Analyze, this.Kinds.ToImmutableArray());
        }

        public void Analyze(GeneratorExecutionContext context, SyntaxNode node)
        {
#pragma warning disable RS1030 // Do not invoke Compilation.GetSemanticModel() method within a diagnostic analyzer
            var semanticModel = context.Compilation.GetSemanticModel(node.SyntaxTree);
#pragma warning restore RS1030 // Do not invoke Compilation.GetSemanticModel() method within a diagnostic analyzer
            foreach (var diagnostic in Analyze(context.Compilation, semanticModel, node, context.CancellationToken))
            {
                context.ReportDiagnostic(diagnostic);
            }
        }

        private void Analyze(SyntaxNodeAnalysisContext context)
        {
            foreach (var diagnostic in Analyze(context.Compilation, context.SemanticModel, context.Node, context.CancellationToken))
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
