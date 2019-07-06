using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Serialization.Serializers.Implementations;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#retroarch")]
    public interface IRetroArchConfig : IConfigurationCollection<IRetroArchConfig>
    {
        [ConfigurationTargetMember("#retroarch")] IVideoConfiguration VideoConfiguration { get; set; }
    }
}
