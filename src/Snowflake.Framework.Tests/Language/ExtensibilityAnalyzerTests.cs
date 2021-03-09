using Snowflake.Language.Analyzers.Extensibility;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Language.Tests
{
    public class ExtensibilityAnalyzerTests
    {
        [Fact]
        public async Task SFC001_PluginMustHavePluginAttributeAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFE001.SFE001.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <PluginMustHavePluginAttributeAnalyzer>
                (testCode, (12, 5), "NonAttributedStandalonePluginImpl"
                );

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC002_PluginMustNotBeNamedCommonAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFE002.SFE002.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <PluginMustNotBeNamedCommonAnalyzer>
                (testCode, (13, 6), "BadStandalonePluginImpl"
                );

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC003_ComposerCallsUnimportedServiceAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFE003.SFE003.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFE003.SFE003.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <ComposerCallsUnimportedServiceAnalyzer, ComposerCallsUnimportedServiceFix>
                (testCode, fixCode, (21, 13), "TestService"
                );

            await harness.RunAsync();
        }
    }
}
