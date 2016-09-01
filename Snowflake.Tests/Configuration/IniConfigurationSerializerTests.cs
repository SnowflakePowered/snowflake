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
            string serializedValue = serializer.Serialize(new ExampleConfigurationSection());
            Assert.Equal(TestUtilities.GetStringResource("Configurations.ExampleConfigurationSection.ini")
                .Replace(Environment.NewLine, ""),
                serializedValue.Replace(Environment.NewLine, ""));
        }
    }
}
