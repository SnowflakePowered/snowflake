using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Configuration.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#dolphin")]
    [ConfigurationTarget("#retroarch")]
    [ConfigurationCollection]
    public partial interface
        OrderSensitiveConfigurationCollection
    {
        [ConfigurationTargetMember("#dolphin")] ExampleConfigurationSection ExampleConfiguration { get; }

        [ConfigurationTargetMember("#retroarch")] IVideoConfiguration VideoConfiguration { get; }
    }
}
