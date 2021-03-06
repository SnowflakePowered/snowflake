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
        public async Task SFC001_UnextendibleInterfaceFix_Section_Test()
        {
            string testCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [ConfigurationSection(""TestSection"", ""TestSection"")]
    public interface TestInterface
    {
    }
}
";
            string fixedCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [ConfigurationSection(""TestSection"", ""TestSection"")]
    public partial interface TestInterface
    {
    }
}
";
            var harness = LanguageTestUtilities.MakeCodeFixTest
                <UnextendibleInterfaceAnalyzer, UnextendibleInterfaceFix>
             (testCode, fixedCode, (6, 5), "TestInterface");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC001_UnextendibleInterfaceFix_Configuration_Test()
        {
            string testCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [ConfigurationCollection]
    public interface TestInterface
    {
    }
}
";
            string fixedCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [ConfigurationCollection]
    public partial interface TestInterface
    {
    }
}
";
            var harness = LanguageTestUtilities.MakeCodeFixTest
                <UnextendibleInterfaceAnalyzer, UnextendibleInterfaceFix>
             (testCode, fixedCode, (6, 5), "TestInterface");

            await harness.RunAsync();
        }

        [Fact]
        public async Task SFC001_UnextendibleInterfaceFix_InputConfiguration_Test()
        {
            string testCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [InputConfiguration(""TestInput"")]
    public interface TestInterface
    {
    }
}
";
            string fixedCode = @"
namespace Snowflake.Framework.Tests.Configuration
{        
    using Snowflake.Configuration;

    [InputConfiguration(""TestInput"")]
    public partial interface TestInterface
    {
    }
}
";
            var harness = LanguageTestUtilities.MakeCodeFixTest
                <UnextendibleInterfaceAnalyzer, UnextendibleInterfaceFix>
             (testCode, fixedCode, (6, 5), "TestInterface");

            await harness.RunAsync();
        }
    }
}
