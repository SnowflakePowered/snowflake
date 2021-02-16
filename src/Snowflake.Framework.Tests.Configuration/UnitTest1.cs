using System;
using Xunit;

namespace Snowflake.Framework.Tests.Configuration
{
    public class UnitTest1 
    {
        [Fact]
        public void Test1()
        {
            var x = ((MyConfiguration)new object()).Descriptor;
        }
    }
}
