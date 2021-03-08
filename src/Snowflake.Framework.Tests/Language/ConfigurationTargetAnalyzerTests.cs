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

        [Fact]
        public async Task SFC009_DuplicateConfigurationTarget_Root_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC009.Root.SFC009.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC009.Root.SFC009.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <DuplicateConfigurationTargetAnalyzer, DuplicateConfigurationTargetFix>
                (testCode, fixCode, 
                    (12, 6), DuplicateConfigurationTargetAnalyzer.RootRule, "#target", "DoubleTargetConfigurationCollection");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC009_DuplicateConfigurationTarget_Null_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC009.Null.SFC009.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC009.Null.SFC009.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <DuplicateConfigurationTargetAnalyzer, DuplicateConfigurationTargetFix>
                (testCode, fixCode,
                    (11, 6), DuplicateConfigurationTargetAnalyzer.RootRule, "#null", "DoubleTargetConfigurationCollection");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC009_DuplicateConfigurationTarget_Edge_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC009.Edge.SFC009.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC009.Edge.SFC009.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <DuplicateConfigurationTargetAnalyzer, DuplicateConfigurationTargetFix>
                (testCode, fixCode,
                    (12, 6), DuplicateConfigurationTargetAnalyzer.EdgeRule, "#target", "child", "DoubleTargetConfigurationCollection");

            await harness.RunAsync();
        }
    }
}
