using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Conventions.Adapters.Types;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ConfigurationQueryBuilder : QueryBuilder
    {
        private IPluginManager PluginManager { get; }
        private IConfigurationCollectionStore Store { get; }
        public ConfigurationQueryBuilder(IConfigurationCollectionStore store, IPluginManager pluginManager)
        {
            this.Store = store;
            this.PluginManager = pluginManager;
        }

        [Field("gameConfiguration", "Gets the emulator configuration the specified game and emulator.", typeof(ConfigurationCollectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "emulatorName", "The plugin name of the emulator.")]
        [Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game of this collection")]
        [Parameter(typeof(string), typeof(StringGraphType), "profileName", "The name of the configuration profile.", nullable: false)]
        public IConfigurationCollection GetEmulatorConfigCollection(string emulatorName, Guid gameGuid, string profileName = "default")
        {
            var emulator = this.PluginManager.Get<IEmulator>(emulatorName);
            var config = emulator.ConfigurationFactory.GetConfiguration(gameGuid, profileName);
            return config;
        }

        [Field("pluginConfiguration", "Gets the plugin configuration options for the specified plugin.",
            typeof(ConfigurationSectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "pluginName", "The name of the plugin.")]
        public IConfigurationSection GetPluginConfiguration(string pluginName)
        {
            var plugin = this.PluginManager.Get(pluginName);
            return plugin.GetPluginConfiguration();
        }

        /*[Mutation("setConfigurationValue", "Config Options", typeof(ConfigurationCollectionGraphType))]
        [Parameter(typeof(IConfigurationValue), typeof(ConfigurationValueGraphType), "value", "The value to set.")]
        public void SetConfigurationValue(IConfigurationValue value)
        {
            this.Store.Set(value);
        }*/
    }
}
