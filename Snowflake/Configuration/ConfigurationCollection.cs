using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationCollection : IConfigurationCollection
    {
        private readonly IDictionary<string, KeyValuePair<IConfigurationSection, Type>> collection = 
            new Dictionary<string, KeyValuePair<IConfigurationSection, Type>>();
        public IEnumerator<KeyValuePair<IConfigurationSection, Type>> GetEnumerator()
        {
            return this.collection.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add<TConfiguration, TSerializer>(string key, TConfiguration configurationSection)
            where TConfiguration : class, IConfigurationSection, new() 
            where TSerializer : class, IConfigurationSerializer, new()
        {
            this.collection.Add(key, new KeyValuePair<IConfigurationSection, Type>(configurationSection, typeof (TSerializer)));
        }

        public TConfiguration Get<TConfiguration>(string key) where TConfiguration : 
            class, IConfigurationSection, new()
        {
            try
            {
                return this.collection[key].Key as TConfiguration;
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }
    }
}
