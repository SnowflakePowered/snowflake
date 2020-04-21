using System;
using System.Collections.Generic;
using GraphQL.Types;
using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Remoting.GraphQL.Attributes;
using Snowflake.Remoting.GraphQL.Query;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Orchestration.Extensibility;
using Snowflake.Services;
using Snowflake.Support.GraphQLFrameworkQueries.Inputs.Configuration;
using Snowflake.Support.GraphQLFrameworkQueries.Types.Configuration;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries
{
    public class ConfigurationQueryBuilder : QueryBuilder
    {
        private IPluginManager PluginManager { get; }
        public IGameLibrary GameLibrary { get; }
        private IPluginConfigurationStore PluginConfigurationStore { get; }

        public ConfigurationQueryBuilder(
            IPluginConfigurationStore pluginConfigurationStore,
            IGameLibrary gameLibrary,
            IPluginManager pluginManager)
        {
            this.PluginConfigurationStore = pluginConfigurationStore;
            //this.GameConfigurationStore = gameConfigurationStore;
            this.PluginManager = pluginManager;
            this.GameLibrary = gameLibrary;
        }

        [Mutation("setGameConfigurationValue", "Config Options", typeof(ListGraphType<ConfigurationValueInputGraphType>))]
        [Parameter(typeof(IEnumerable<ConfigurationValueInputObject>), typeof(ListGraphType<ConfigurationValueInputType>), "input", "The value to set.")]
        public IEnumerable<IConfigurationValue> SetGameConfigurationValue(IEnumerable<ConfigurationValueInputObject> input)
        {
            var configStore = this.GameLibrary.GetExtension<IGameConfigurationExtensionProvider>();
            foreach (var value in input)
            {
                configStore.UpdateValue(value.Guid, value.Value);
            }
            return input;
        }

        [Mutation("setPluginConfigurationValue", "Sets a specific plugin configuration value.",
            typeof(ListGraphType<ConfigurationValueInputGraphType>))]
        [Parameter(typeof(IEnumerable<ConfigurationValueInputObject>),
            typeof(ListGraphType<ConfigurationValueInputType>), "input", "The value to set.")]
        public IEnumerable<IConfigurationValue> SetPluginConfigurationValue(
            IEnumerable<ConfigurationValueInputObject> input)
        {
            this.PluginConfigurationStore.Set(input);
            return input;
        }
    }
}
