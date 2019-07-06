using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Serialization.Serializers.Implementations;
using Snowflake.Configuration.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#dolphin", typeof(SimpleIniConfigurationSerializer))]
    [ConfigurationTarget("#retroarch", typeof(SimpleCfgConfigurationSerializer))]
    public interface
        OrderSensitiveConfigurationCollection : IConfigurationCollection<OrderSensitiveConfigurationCollection>
    {
        [ConfigurationTargetMember("#dolphin")] ExampleConfigurationSection ExampleConfiguration { get; set; }

        [ConfigurationTargetMember("#retroarch")] IVideoConfiguration VideoConfiguration { get; set; }
    }
}
