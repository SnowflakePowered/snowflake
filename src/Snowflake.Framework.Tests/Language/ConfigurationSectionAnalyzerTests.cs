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
                (testCode, (18, 13), "TestInterface");
            
            await harness.RunAsync();
        }
    }
}
