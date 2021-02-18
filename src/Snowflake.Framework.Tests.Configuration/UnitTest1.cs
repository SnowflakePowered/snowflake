using Snowflake.Configuration;
using Snowflake.Configuration.Tests;
using System;
using Xunit;

namespace Snowflake.Framework.Tests.Configuration
{
    public class UnitTest1 
    {
        [Fact]
        public void Test1()
        {
           
            var x = new ConfigurationSection<MyConfiguration>();
            var y = new ConfigurationCollection<ExampleConfigurationCollection>();
            x.Configuration.MyBoolean = true;
            
        }
    }
}
