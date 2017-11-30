using Snowflake.Configuration;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ConfigurationQueryBuilder : QueryBuilder
    {
        private IConfigurationCollectionStore Store { get; }
        public ConfigurationQueryBuilder(IConfigurationCollectionStore store)
        {
            this.Store = store;
        }
        [Connection("configValues", "Config Values", typeof(ConfigurationValueType))]
        public IEnumerable<KeyValuePair<string, IConfigurationValue>> GetAllValues()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            return config.Configuration.TestConfiguration.Values.ToList();
        }
        //            
        [Connection("configOptions", "Config Options", typeof(ConfigurationOptionType))]
        public IEnumerable<IConfigurationOption> GetAllOptions()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            return config.Configuration.TestConfiguration.Descriptor.Options;
        }
    }
}
