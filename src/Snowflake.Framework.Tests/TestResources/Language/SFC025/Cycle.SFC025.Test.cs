using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("TestCycle1", "TestCycle2")]
    [ConfigurationTarget("TestCycle2", "TestCycle1")]
    [ConfigurationCollection]
    public partial interface TestCollection
    {
    }
}
