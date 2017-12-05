using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Tests;
using Xunit;
namespace Snowflake.Configuration.Tests
{
    public class IniConfigurationSerializerTests
    {
        [Fact]
        public void IniConfigurationSerializer_SerializeTest()
        {
            var serializer = new IniConfigurationSerializer(new BooleanMapping("true", "false"), "null");
            var config = new ConfigurationCollection<ExampleConfigurationCollection>();
            string serializedValue = serializer.Serialize(config.Configuration.ExampleConfiguration);
            Assert.Equal(TestUtilities.GetStringResource("Configurations.ExampleConfigurationSection.ini")
                .Replace(Environment.NewLine, string.Empty),
                serializedValue.Replace(Environment.NewLine, string.Empty));
        }
    }
}