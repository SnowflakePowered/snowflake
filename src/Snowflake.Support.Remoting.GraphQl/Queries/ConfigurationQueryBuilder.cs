using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Execution.Extensibility;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Configuration;
using Snowflake.Framework.Remoting.GraphQl.Attributes;
using Snowflake.Framework.Remoting.GraphQl.Query;
using Snowflake.Services;
using Snowflake.Support.Remoting.GraphQl.Inputs.Configuration;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ConfigurationQueryBuilder : QueryBuilder
    {
        private IPluginManager PluginManager { get; }
        private IPluginConfigurationStore PluginConfigurationStore { get; }
        public ConfigurationQueryBuilder(
            IPluginConfigurationStore pluginConfigurationStore,
            IPluginManager pluginManager)
        {
            this.PluginConfigurationStore = pluginConfigurationStore;
            //this.GameConfigurationStore = gameConfigurationStore;
            this.PluginManager = pluginManager;
        }

        //[Field("gameConfiguration", "Gets the emulator configuration the specified game and emulator.", typeof(ConfigurationCollectionGraphType))]
        //[Parameter(typeof(string), typeof(StringGraphType), "emulatorName", "The plugin name of the emulator.")]
        //[Parameter(typeof(Guid), typeof(GuidGraphType), "gameGuid", "The GUID of the game of this collection")]
        //[Parameter(typeof(string), typeof(StringGraphType), "profileName", "The name of the configuration profile.", nullable: false)]
        //public IConfigurationCollection GetEmulatorConfigCollection(string emulatorName, Guid gameGuid, string profileName = "default")
        //{
        //    var emulator = this.PluginManager.Get<IEmulator>(emulatorName);
        //    var config = emulator.ConfigurationFactory.GetConfiguration(gameGuid, profileName);
        //    return config;
        //}

        [Field("pluginConfiguration", "Gets the plugin configuration options for the specified plugin.",
            typeof(ConfigurationSectionGraphType))]
        [Parameter(typeof(string), typeof(StringGraphType), "pluginName", "The name of the plugin.")]
        public IConfigurationSection GetPluginConfiguration(string pluginName)
        {
            var plugin = this.PluginManager.Get(pluginName);
            return plugin.GetPluginConfiguration();
        }

        //[Mutation("setGameConfigurationValue", "Config Options", typeof(ConfigurationValueInputGraphType))]
        //[Parameter(typeof(IEnumerable<ConfigurationValueInputObject>), typeof(ListGraphType<ConfigurationValueInputType>), "input", "The value to set.")]
        //public IEnumerable<IConfigurationValue> SetGameConfigurationValue(IEnumerable<ConfigurationValueInputObject> input)
        //{
        //    // this.GameConfigurationStore.Set(input);
        //    return input;
        //}

        [Mutation("setPluginConfigurationValue", "Config Options", typeof(ConfigurationValueInputGraphType))]
        [Parameter(typeof(IEnumerable<ConfigurationValueInputObject>), typeof(ListGraphType<ConfigurationValueInputType>), "input", "The value to set.")]
        public IEnumerable<IConfigurationValue> SetPluginConfigurationValue(IEnumerable<ConfigurationValueInputObject> input)
        {
            this.PluginConfigurationStore.Set(input);
            return input;
        }
    }
}
