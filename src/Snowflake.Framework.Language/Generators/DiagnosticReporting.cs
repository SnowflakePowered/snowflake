using Microsoft.CodeAnalysis;
using Snowflake.Language.Analyzers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Language.Generators.Configuration
{
    internal static class DiagnosticReporting
    {
        static DiagnosticDescriptor FrameworkMissing = new DiagnosticDescriptor(
            DiagnosticCodes.SFG000__GenericDiagnosticError,
            title: "Snowflake Framework APIs missing",
            messageFormat: "Required APIs from Snowflake.Framework could not be found.",
            category: "Snowflake.Language",
            DiagnosticSeverity.Error,
            true,
            customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        internal static void ReportFrameworkMissing(this GeneratorExecutionContext context)
        {
            context.ReportDiagnostic(Diagnostic.Create(FrameworkMissing, Location.None));
        }
    }
}
