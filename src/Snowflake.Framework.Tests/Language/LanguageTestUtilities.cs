using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Language.Tests
{
    public static class LanguageTestUtilities
    {
        private static readonly MetadataReference SnowflakePrimitiveReference 
            = MetadataReference.CreateFromFile(typeof(ConfigurationCollectionAttribute).Assembly.Location);
        private static readonly MetadataReference SnowflakeFrameworkReference 
            = MetadataReference.CreateFromFile(typeof(ConfigurationCollection<>).Assembly.Location);
        private static readonly MetadataReference SnowflakeTestReference
          = MetadataReference.CreateFromFile(typeof(LanguageTestUtilities).Assembly.Location);
        private static Solution AddSnowflakeReferences(Solution solution, ProjectId projectId)
        {
            Project project = solution.GetProject(projectId)!;
            var parseOptions = (CSharpParseOptions)project.ParseOptions!;
            project = project.WithParseOptions(parseOptions.WithLanguageVersion(LanguageVersion.CSharp9));
            project = project
                .AddMetadataReference(SnowflakePrimitiveReference)
                .AddMetadataReference(SnowflakeFrameworkReference)
                .AddMetadataReference(SnowflakeTestReference);
            return project.Solution;
        }

        public static CSharpAnalyzerTest<TAnalyzer, XUnitVerifier> 
            MakeAnalyzerTest<TAnalyzer>(string testSource, (int line, int col) location, params object[] arguments)
            where TAnalyzer : DiagnosticAnalyzer, new()
        {
            return new CSharpAnalyzerTest<TAnalyzer, XUnitVerifier>()
            {
                TestCode = testSource,
                ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
                SolutionTransforms =
                {
                    LanguageTestUtilities.AddSnowflakeReferences,
                },
                TestState =
                {
                    ExpectedDiagnostics = { 
                        Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<TAnalyzer>.Diagnostic()
                        .WithArguments(arguments)
                        .WithLocation(location.line, location.col)
                    },
                },
            };
        }
    }
}
