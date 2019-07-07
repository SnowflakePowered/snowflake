using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration.Serialization.Serializers;
using Xunit;

namespace Snowflake.Configuration.Serialization
{
    public class StringSerializationContextTests
    {
        [Fact]
        public void Scope_Tests()
        {
           var context = new StringSerializationContext();
            Assert.Equal(string.Empty, context.ExitScope());
            Assert.Equal(string.Empty, context.GetCurrentScope());

        }
    }
}
