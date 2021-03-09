using Snowflake.Language.Analyzers.Configuration;
using Snowflake.Language.Tests;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Language.Tests
{
    public class InputConfigurationAnalyzerTests
    {
        [Fact]
        public async Task SFC014_InputPropertyMustHaveGetter_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC014.InputConfigurationTest.SFC014.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC014.InputConfigurationTest.SFC014.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <SectionPropertyMustHaveGetterAnalyzer, SectionPropertyMustHaveGetterFix>
                (testCode, fixCode, (11, 9), SectionPropertyMustHaveGetterAnalyzer.InputRule, "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC016_InputPropertyInvalidAccessorAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC016.SFC016.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC016.SFC016.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <InputPropertyInvalidAccessorAnalyzer, InputPropertyInvalidAccessorFix>
                (testCode, fixCode, (12, 46), "TestProperty", "set");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC023_GenericArgumentRequiresInputConfigurationAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC023.SFC023.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                   <GenericArgumentRequiresInputConfigurationAnalyzer>
                   (testCode,
                       (46, 13, new[] { "TestInterface" }),
                       (47, 13, new[] { "TestInterface" }),
                       (47, 33, new[] { "TestInterface" }),
                       (48, 13, new[] { "TestInterface" }),
                       (48, 53, new[] { "TestInterface" }),
                       (49, 13, new[] { "ImplementingClass" })
                   );

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC017_InputPropertyUndecorated_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC017.SFC017.Test.cs");

            var harness = LanguageTestUtilities.MakeAnalyzerTest<InputPropertyUndecoratedAnalyzer>
                (testCode, (9, 16), "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC018_InputPropertyTypeMismatch_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC018.SFC018.Test.cs");

            var harness = LanguageTestUtilities.MakeAnalyzerTest<InputPropertyTypeMismatchAnalyzer>
                (testCode, (12, 16), "TestProperty", "string");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC019_OnlyOneAttributeTemplateType_Test()
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC019.SFC019.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest<OnlyOneAttributeTemplateTypeAnalyzer>
               (testCode, (8, 30), "TestInterface");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC020_OnlyOneAttributePropertyType_Test()
        {
            string testCode = TestUtilities.GetStringResource($"Language.SFC020.SFC020.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest<OnlyOneAttributePropertyTypeAnalyzer>
               (testCode, (13, 26), "TestProperty");

            await harness.RunAsync();
        }
    }
}
