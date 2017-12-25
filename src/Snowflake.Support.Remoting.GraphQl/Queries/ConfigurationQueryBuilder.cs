using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public ConfigurationQueryBuilder(IConfigurationCollectionStore store)
        {
            this.Store = store;
        }

        [Field("configurationCollection", "Config Options", typeof(ConfigurationCollectionGraphType))]
        public IConfigurationCollection GetCollection(string emulatorName, Guid gameGuid, string profileName = "default")
        {
            var emulator = this.PluginManager.Get<IEmulator>(emulatorName);
            var config = emulator.ConfigurationFactory.GetConfiguration(gameGuid, profileName);
            return config;
        }

        [Mutation("setConfigurationValue", "Config Options", typeof(ConfigurationCollectionGraphType))]
        public void SetConfigurationValue(IConfigurationValue value)
        {
            this.Store.Set(value);
        }
    }
}
