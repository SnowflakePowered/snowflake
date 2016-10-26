using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.DynamicConfiguration;

namespace Snowflake.Tests.DynamicConfig
{
    [ConfigurationFile("#retroarch", "retroarch.cfg")]
    public interface IRetroArchConfig : IConfigurationCollection<IRetroArchConfig>
    {
        [ConfigurationSection("video", "Video Options", "#retroarch")]
        IVideoConfiguration VideoConfiguration { get; set; }
    }
}
