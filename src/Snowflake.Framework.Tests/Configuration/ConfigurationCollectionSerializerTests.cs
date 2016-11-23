using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Tests;
using Xunit;
namespace Snowflake.Configuration.Tests
{
    public class ConfigurationCollectionSerializerTests
    {
        [Fact]
        public void JsonSerialization_Test()
        {
            JObject jobject = 
                JsonConvert.DeserializeObject<JObject>
                (JsonConvert.SerializeObject(new ConfigurationCollection<ExampleConfigurationCollection>())).Children<JProperty>().First().Value as JObject;
            Assert.Contains("Values", jobject.Properties().Select(k => k.Name));
            Assert.Contains("Options", jobject.Properties().Select(k => k.Name));
            Assert.Contains("Selections", jobject.Properties().Select(k => k.Name));


        }
    }
}
