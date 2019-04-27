using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Extensibility.Tests
{
    public class JsonPluginPropertiesTest
    {
        [Fact]
        public void JsonPluginProperties_Tests()
        {
            var propRoot =
                JsonConvert.DeserializeObject<JObject>(TestUtilities.GetStringResource("Loader.plugin.json"));
            IPluginProperties properties = new JsonPluginProperties(propRoot);
            Assert.Equal("TestString", properties.Get("someString"));
            Assert.Contains("One", properties.GetEnumerable("someArray"));
            Assert.Contains("Two", properties.GetEnumerable("someArray"));
            Assert.Contains("one", properties.GetDictionary("someDictionary").Keys);
            Assert.Contains("two", properties.GetDictionary("someDictionary").Keys);
        }

        [Fact]
        public void JsonPluginProperties_InvalidTests()
        {
            var propRoot =
                JsonConvert.DeserializeObject<JObject>(TestUtilities.GetStringResource("Loader.plugin.json"));
            IPluginProperties properties = new JsonPluginProperties(propRoot);
            Assert.Equal(String.Empty, properties.Get("notInObject"));
            Assert.Empty(properties.GetEnumerable("notInObject"));
            Assert.Empty(properties.GetDictionary("notInObject"));
        }
    }
}
