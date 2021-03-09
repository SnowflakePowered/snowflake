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
    public class ConfigurationSectionAnalyzerTests
    {
        [Fact]
        public async Task SFC022_GenericArgumentRequiresConfigurationSectionAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC022.SFC022.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <GenericArgumentRequiresConfigurationSectionAnalyzer>
                (testCode, 
                    (39, 13, new[] { "TestInterface" }),
                    (40, 13, new[] { "TestInterface" }),
                    (40, 33, new[] { "TestInterface" }),
                    (41, 13, new[] { "TestInterface" }),
                    (41, 53, new[] { "TestInterface" }),
                    (42, 13, new[] { "ImplementingClass" })
                );
            
            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC013_SectionPropertyInvalidAccessorAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC013.SFC013.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC013.SFC013.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <SectionPropertyInvalidAccessorAnalyzer, SectionPropertyInvalidAccessorFix>
                (testCode, fixCode, (9, 36), "TestProperty", "init");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC014_CollectionPropertyMustHaveGetter_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC014.SectionTest.SFC014.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC014.SectionTest.SFC014.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <SectionPropertyMustHaveGetterAnalyzer, SectionPropertyMustHaveGetterFix>
                (testCode, fixCode, (8, 9), SectionPropertyMustHaveGetterAnalyzer.SectionRule, "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC015_CollectionPropertyMustHaveSetter_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC015.SFC015.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC015.SFC015.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <SectionPropertyMustHaveSetterAnalyzer, SectionPropertyMustHaveSetterFix>
                (testCode, fixCode, (8, 9), "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC010_SectionPropertyUndecorated_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC010.SFC010.Test.cs");

            var harness = LanguageTestUtilities.MakeAnalyzerTest<SectionPropertyUndecoratedAnalyzer>
                (testCode, (8, 16), "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC012_SectionPropertyEnumUndecorated_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC012.SFC012.Test.cs");

            var harness = LanguageTestUtilities.MakeAnalyzerTest<SectionPropertyEnumUndecoratedAnalyzer>
                (testCode, (9, 18), "TestProperty", "Snowflake.Framework.Tests.Configuration.TestEnum");

            await harness.RunAsync();
        }
    }
}
