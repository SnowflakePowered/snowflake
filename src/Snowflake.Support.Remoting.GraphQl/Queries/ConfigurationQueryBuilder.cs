using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Plugin.Emulators.TestEmulator.Configuration;
using Snowflake.Support.Remoting.GraphQl.Framework.Attributes;
using Snowflake.Support.Remoting.GraphQl.Framework.Query;
using Snowflake.Support.Remoting.GraphQl.Types.Configuration;

namespace Snowflake.Support.Remoting.GraphQl.Queries
{
    public class ConfigurationQueryBuilder : QueryBuilder
    {
        private IConfigurationCollectionStore Store { get; }
        public ConfigurationQueryBuilder(IConfigurationCollectionStore store)
        {
            this.Store = store;
        }

        [Connection("configValues", "Config Values", typeof(ConfigurationValueGraphType))]
        public IEnumerable<KeyValuePair<string, IConfigurationValue>> GetAllValues()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            return config.Configuration.TestConfiguration.Values.ToList();
        }

        [Connection("configOptions", "Config Options", typeof(ConfigurationOptionDescriptorGraphType))]
        public IEnumerable<IConfigurationOptionDescriptor> GetAllOptions()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            return config.Configuration.TestConfiguration.Descriptor.Options;
        }

        [Connection("configSections", "Config Options", typeof(ConfigurationSectionGraphType))]
        public IEnumerable<KeyValuePair<string, IConfigurationSection>> GetAllSections()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            return config.Configuration;
        }

        [Connection("configCollection", "Config Options", typeof(ConfigurationCollectionGraphType))]
        public IEnumerable<IConfigurationCollection> GetCollection()
        {
            var config = this.Store.Get<ITestConfigurationCollection>(Guid.NewGuid(), "TestEmulator", "DefaultProfile");
            yield return config;
        }
    }
}
