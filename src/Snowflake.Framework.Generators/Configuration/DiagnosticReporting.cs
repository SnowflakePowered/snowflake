using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Configuration.Generators
{
    internal static class DiagnosticReporting
    {
        internal static void ReportError(this GeneratorExecutionContext context, DiagnosticError code, string title, string message, Location location, ref bool errorOccured)
        {
            var diag =  Diagnostic.Create(
                        new DiagnosticDescriptor($"SFC{(int)code}", title,
                            message,
                            "Snowflake.Configuration.Generators", DiagnosticSeverity.Error, true),
                        location);
            context.ReportDiagnostic(diag);
            errorOccured = true;
        }
    }

    internal enum DiagnosticError
    {
        GeneralError = 1000,
        InvalidMembers = 1001,
        UnextendibleInterface = 1002,
        UndecoratedProperty = 1003,
        MismatchedType = 1004,
        UnexpectedSetter = 1005,
        UnexpectedBody = 1006,
        MissingSetter = 1007,
        MissingGetter = 1008,
        NotAConfigurationSection = 1009,
        NotTopLevel = 1010,
        UndecoratedEnum = 1011,
        DuplicatedTarget = 1012,
        FrameworkNotFound = 1111,
    }
}
