using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration;

namespace Snowflake.Configuration.Tests
{
    [ConfigurationFile("#retroarch", "retroarch.cfg")]
    public interface IRetroArchConfig : IConfigurationCollection<IRetroArchConfig>
    {
        [SerializableSection("#retroarch")]
        IVideoConfiguration VideoConfiguration { get; set; }
    }
}
