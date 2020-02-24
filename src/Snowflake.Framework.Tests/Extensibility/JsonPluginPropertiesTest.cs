using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Support.PluginManager;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Extensibility.Tests
{
    public class JsonPluginPropertiesTest
    {
        [Fact]
        public void JsonPluginProperties_Tests()
        {
            var opts = new JsonSerializerOptions();
            opts.Converters.Add(new PluginPropertiesConverter());
            var props = 
                JsonSerializer.Deserialize<PluginPropertiesData>(TestUtilities.GetStringResource("Loader.plugin.json"), opts);
            IPluginProperties properties = new PluginProperties(props.Strings, props.Dictionaries, props.Arrays);

            Assert.Equal("TestString", properties.Get("someString"));
            Assert.Contains("One", properties.GetEnumerable("someArray"));
            Assert.Contains("Two", properties.GetEnumerable("someArray"));
            Assert.Contains("one", properties.GetDictionary("someDictionary").Keys);
            Assert.Contains("two", properties.GetDictionary("someDictionary").Keys);
        }

        [Fact]
        public void JsonPluginProperties_InvalidTests()
        {
            var opts = new JsonSerializerOptions();
            opts.Converters.Add(new PluginPropertiesConverter());
            var props =
                JsonSerializer.Deserialize<PluginPropertiesData>(TestUtilities.GetStringResource("Loader.plugin.json"), opts);
            IPluginProperties properties = new PluginProperties(props.Strings, props.Dictionaries, props.Arrays);

            Assert.Equal(String.Empty, properties.Get("notInObject"));
            Assert.Empty(properties.GetEnumerable("notInObject"));
            Assert.Empty(properties.GetDictionary("notInObject"));
        }
    }
}
