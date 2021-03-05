using Snowflake.Language.Analyzers.Configuration;
using Snowflake.Language.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Language.Tests
{
    public class ConfigurationAnalyzerTests
    {
        [Fact]
        public async Task SFC022_GenericArgumentRequiresConfigurationSectionAnalyzer_Test()
        {
            string testCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
        using Snowflake.Configuration.Internal;

        public interface SomeInterface
        {
        }

        public class TestFixture
        {
            [GenericTypeAcceptsConfigurationSection(0)]
            public void Use<T>()
                where T: class
            {
            }
            public void Else()
            {
                this.Use<SomeInterface>();
            }
        }
}
";
            var harness = LanguageTestUtilities.MakeAnalyzerTest<GenericArgumentRequiresConfigurationSectionAnalyzer>
                (testCode, (19, 17), "SomeInterface");
            await harness.RunAsync();
        }
    }
}
