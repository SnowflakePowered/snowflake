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
    public class ConfigurationCollectionAnalyzerTests
    {
        [Fact]
        public async Task SFC006_CollectionPropertyNotSectionAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC006.SFC006.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <CollectionPropertyNotSectionAnalyzer>
                (testCode, (12, 21), "Section");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC007_CollectionPropertyInvalidAccessorAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC007.SFC007.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC007.SFC007.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <CollectionPropertyInvalidAccessorAnalyzer, CollectionPropertyInvalidAccessorFix>
                (testCode, fixCode, (8, 36), "TestProperty", "set");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC008_CollectionPropertyMustHaveGetter_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC008.SFC008.Test.cs");
            string fixCode = TestUtilities.GetStringResource("Language.SFC008.SFC008.Fix.cs");

            var harness = LanguageTestUtilities.MakeCodeFixTest
                <CollectionPropertyMustHaveGetterAnalyzer, CollectionPropertyMustHaveGetterFix>
                (testCode, fixCode, (8, 9), "TestProperty");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC021_GenericArgumentRequiresConfigurationCollectionAnalyzer_Test()
        {
            string testCode = TestUtilities.GetStringResource("Language.SFC021.SFC021.Test.cs");
            var harness = LanguageTestUtilities.MakeAnalyzerTest
                <GenericArgumentRequiresConfigurationCollectionAnalyzer>
                (testCode, (20, 13), "TestInterface");

            await harness.RunAsync();
        }
    }
}
