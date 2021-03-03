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
    public sealed class GenericArgumentRequiresInputConfigurationAnalyzer
        : AbstractGenericTypeRequiresAttributeAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        protected override DiagnosticDescriptor Rule => _Rule;

        private static readonly DiagnosticDescriptor _Rule =
            new DiagnosticDescriptor(
                id: DiagnosticCodes.SFC021__GenericArgumentExpectsConfigurationCollection,
                title: "The specified generic argument expects a InputConfiguration template interface.",
                messageFormat: "Type '{0}' must be a InputConfiguration template interface.",
                category: "Snowflake.Configuration",
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                customTags: new[] { WellKnownDiagnosticTags.NotConfigurable },
                description: "The specified generic argument must be an interface with the [InputConfiguration] attribute.");

        protected override (INamedTypeSymbol marker, INamedTypeSymbol target) GetAttributes(Compilation compilation)
        {
            var types = new ConfigurationTypes(compilation);

            return (types.GenericTypeAcceptsInputConfigurationAttribute, types.InputConfigurationAttribute);
        }
    }
}
