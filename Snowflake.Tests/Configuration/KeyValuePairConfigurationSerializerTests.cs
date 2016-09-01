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
    public class KeyValuePairConfigurationSerializerTests
    {
       [Fact]
        public void KeyValuePairConfigurationSerializer_SerializeTest()
        {
            var serializer = new KeyValuePairConfigurationSerializer(new BooleanMapping("true", "false"), "null", "=");
            string serializedValue = serializer.Serialize(new ExampleConfigurationSection());
            Assert.Equal(TestUtilities.GetStringResource("Configurations.ExampleConfigurationSection.cfg")
                .Replace(Environment.NewLine, ""),
                serializedValue.Replace(Environment.NewLine, ""));
        }
    }
}
