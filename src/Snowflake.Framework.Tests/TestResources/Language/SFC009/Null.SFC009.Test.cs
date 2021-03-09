using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#null")]
    [ConfigurationCollection]
    public partial interface DoubleTargetConfigurationCollection
    {
    }
}
