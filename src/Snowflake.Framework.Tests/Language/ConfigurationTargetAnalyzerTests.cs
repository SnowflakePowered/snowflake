using Snowflake.Language.Analyzers.Configuration;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Language.Tests
{
    public class ConfigurationTargetAnalyzerTests
    {
        [Fact]
        public async Task SFC024_ConfigurationTargetDoesNotExistMember_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC024.Member.SFC024.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC024.Member.SFC024.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <ConfigurationTargetDoesNotExistAnalyzer, ConfigurationTargetDoesNotExistFix>
                (testCode, fixCode, (8, 10), "TestTarget");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC024_ConfigurationTargetDoesNotExistChild_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC024.Child.SFC024.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC024.Child.SFC024.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <ConfigurationTargetDoesNotExistAnalyzer, ConfigurationTargetDoesNotExistFix>
                (testCode, fixCode, (6, 6), "TestTarget");

            await harness.RunAsync();
        }

        [Theory]
        [InlineData("Root", "#target", 12)]
        [InlineData("Null", "#null", 11)]
        [InlineData("Cycle", "TestCycle2", 14)]
        [InlineData("Edge", "child", 12)]
        public async Task SFC009_ConfigurationTargetAlreadyExists_Root_Test(string file, string target, int col)
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC009.{file}.SFC009.Test.cs");
            string fixCode = TestUtilities.GetStringResource($"Language.SFC009.{file}.SFC009.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <ConfigurationTargetAlreadyExistsAnalyzer, ConfigurationTargetAlreadyExistsFix>
                (testCode, fixCode, 
                    (col, 6),  target, "DoubleTargetConfigurationCollection");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC025_ConfigurationTargetNotRooted_NoCycle_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC025.NoCycle.SFC025.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <ConfigurationTargetNotRootedAnalyzers>
                (testCode);

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC025_ConfigurationTargetNotRooted_Cycle_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC025.Cycle.SFC025.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <ConfigurationTargetNotRootedAnalyzers>
                (testCode,
                    (11, 6, new[] { "TestCycle1", "TestCollection" }),
                    (12, 6, new[] { "TestCycle2", "TestCollection" })
                );

            await harness.RunAsync();
        }
    }
}
