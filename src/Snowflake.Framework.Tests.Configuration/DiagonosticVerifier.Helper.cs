using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Snowflake.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Framework.Tests.Configuration
{
    public class DiagonosticVerifier
    {
        private static readonly MetadataReference SnowflakePrimitiveReference = MetadataReference.CreateFromFile(typeof(ConfigurationCollectionAttribute).Assembly.Location);
        private static readonly MetadataReference SnowflakeFrameworkReference = MetadataReference.CreateFromFile(typeof(ConfigurationCollection<>).Assembly.Location);

        public static Solution AddSnowflakeReferences(Solution solution, ProjectId projectId)
        {
            Project project = solution.GetProject(projectId)!;
            var parseOptions = (CSharpParseOptions)project.ParseOptions!;
            project = project.WithParseOptions(parseOptions.WithLanguageVersion(LanguageVersion.CSharp9));
            project = project
                .AddMetadataReference(SnowflakePrimitiveReference)
                .AddMetadataReference(SnowflakeFrameworkReference);
            return project.Solution;
        }
    }
}
