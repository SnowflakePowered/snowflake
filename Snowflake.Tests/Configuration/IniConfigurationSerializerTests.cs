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
            var serializer = new IniConfigurationSerializer(new BooleanMapping("true", "false"), "null", true);
            string serializedValue = serializer.Serialize(new ExampleConfigurationSection());
            File.WriteAllText("null.text", serializedValue);
            Assert.Equal(TestUtilities.GetStringResource("Configurations.ExampleConfigurationSection.ini"),
                serializedValue);
        }

        [Fact]
        public void IniConfigurationSerializer_SerializeIterableTest()
        {
            var serializer = new IniConfigurationSerializer(new BooleanMapping("true", "false"), "null", true);
            string serializedValue = serializer.Serialize(new ExampleIterableConfigurationSection(278));
            Assert.Equal(TestUtilities.GetStringResource("Configurations.ExampleIterableConfigurationSection.ini"),
                serializedValue);
        }
    }
}
