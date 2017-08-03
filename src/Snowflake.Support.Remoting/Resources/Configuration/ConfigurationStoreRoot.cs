using Snowflake.Configuration;
using Snowflake.Remoting.Resources;
using Snowflake.Remoting.Resources.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Resources.Configuration
{
    [Resource("configuration", ":configGuid")]
    public class ConfigurationStoreRoot : Resource
    { 
        public IConfigurationCollectionStore CollectionStore { get; }
        public ConfigurationStoreRoot(IConfigurationCollectionStore collectionStore)
        {

        }

        [Endpoint(EndpointVerb.Update)]
        [Parameter(typeof(object), "newValue")]
        public IConfigurationValue SetValue(IConfigurationValue configValue, object newValue)
        {
            configValue.Value = newValue;
            this.CollectionStore.Set(configValue);
            return configValue;
        }
    }
}
