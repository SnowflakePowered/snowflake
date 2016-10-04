using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Xunit;

namespace Snowflake.Configuration.Tests
{
    public class ConfigurationSectionTests
    {
        [Fact]
        public void ConfigurationSectionEnum_Test()
        {
            var config = new ExampleConfigurationSection();
            foreach (var option in config.Options)
            {
                Assert.Equal(config.GetType().GetProperty(option.Key).GetValue(config), option.Value.GetValue(Guid.Empty).Value);
            }
        }

        [Fact]
        public void ConfigurationSectionSetter_Test()
        {
            var config = new ExampleConfigurationSection();
            foreach (var option in config.Options)
            {
                var objType = option.Value.GetValue(Guid.Empty)?.Value.GetType();
                option.Value.GetValue(Guid.Empty).Value = null;
                Assert.Equal(config.GetType().GetProperty(option.Key).GetValue(config), 
                    objType.IsValueType || objType.IsEnum
                    ? Activator.CreateInstance(objType) : null);
            }
        }
    }
}
