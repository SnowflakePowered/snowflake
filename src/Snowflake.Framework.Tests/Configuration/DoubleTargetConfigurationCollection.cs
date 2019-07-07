using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Serialization.Serializers.Implementations;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#target")]
    [ConfigurationTarget("#target")]
    [ConfigurationTarget("#target2", "#target")]
    [ConfigurationTarget("#target2", "#target")]
    public interface DoubleTargetConfigurationCollection : IConfigurationCollection<DoubleTargetConfigurationCollection>
    {
        [ConfigurationTargetMember("#target")]
        ExampleConfigurationSection ExampleConfiguration { get; }
    }
}
