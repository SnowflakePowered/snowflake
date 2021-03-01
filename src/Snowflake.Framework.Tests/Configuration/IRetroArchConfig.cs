using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Serialization.Serializers.Implementations;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationTarget("#retroarch")]
    [ConfigurationCollection]
    public partial interface IRetroArchConfig 
    {
        [ConfigurationTargetMember("#retroarch")] IVideoConfiguration VideoConfiguration { get; }
    }
}
