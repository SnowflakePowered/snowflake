using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Generators
{
    internal static class DiagnosticReporting
    {
        internal static void ReportError(this GeneratorExecutionContext context, int code, string title, string message, Location location, ref bool errorOccured)
        {
            var diag =  Diagnostic.Create(
                        new DiagnosticDescriptor($"SFC{code}", title,
                            message,
                            "Snowflake.Configuration.Generators", DiagnosticSeverity.Error, true),
                        location);
            context.ReportDiagnostic(diag);
            errorOccured = true;
        }
    }
}
