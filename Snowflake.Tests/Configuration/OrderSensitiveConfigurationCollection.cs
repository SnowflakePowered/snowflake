using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Tests;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationFile("#dolphin", "Dolphin.ini")]
    [ConfigurationFile("#retroarch", "retroarch.cfg")]
    public interface OrderSensitiveConfigurationCollection : IConfigurationCollection<OrderSensitiveConfigurationCollection>
    {
        [SerializableSection("#dolphin")]
        ExampleConfigurationSection ExampleConfiguration { get; set; }

        [SerializableSection("#retroarch")]
        IVideoConfiguration VideoConfiguration { get; set; }

    }
}
