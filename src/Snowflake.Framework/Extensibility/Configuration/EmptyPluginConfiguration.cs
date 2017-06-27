using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Extensibility
{
    [ConfigurationSection("EmptyPluginConfiguration", "Default Plugin Configuration")]
    public interface IEmptyPluginConfiguration : IConfigurationSection<IEmptyPluginConfiguration>
    {
    }

    internal static class EmptyPluginConfiguration
    {
        /// <summary>
        /// The empty plugin configuration.
        /// </summary>
        public static IConfigurationSection EmptyConfiguration = new ConfigurationSection<IEmptyPluginConfiguration>();
    }
}
