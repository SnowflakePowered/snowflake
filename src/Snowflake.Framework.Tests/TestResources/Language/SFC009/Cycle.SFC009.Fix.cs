using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("TestCycle1")]
    [ConfigurationTarget("TestCycle2", "TestCycle1")]
    [ConfigurationTarget("TestCycle3", "TestCycle2")]
    [ConfigurationCollection]
    public partial interface DoubleTargetConfigurationCollection
    {
    }
}
