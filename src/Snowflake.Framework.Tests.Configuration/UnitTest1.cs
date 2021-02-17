using Snowflake.Configuration;
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

            x.Configuration.MyBoolean = true;
        }
    }
}
