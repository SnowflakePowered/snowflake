using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class ConfigurationCollectionDescriptor<T> : IConfigurationCollectionDescriptor where T: class, IConfigurationCollection
    {
        public IDictionary<string, string> Outputs { get; }
        public IList<string> SectionKeys { get; }
        private IDictionary<string, string> DestinationMappings { get; }

        public ConfigurationCollectionDescriptor()
        {
            this.Outputs = ImmutableDictionary.CreateRange(typeof(T).GetCustomAttributes<ConfigurationFileAttribute>()
                .ToDictionary(f => f.Key, f => f.FileName));
            var sections =
                (from props in typeof(T).GetProperties()
                    where props.HasAttribute<SerializableSectionAttribute>()
                    select props).ToImmutableList();
            this.SectionKeys = ImmutableList.CreateRange(from props in sections select props.Name);

            this.DestinationMappings = ImmutableDictionary.CreateRange(from props in sections
                let destination = props.GetAttribute<SerializableSectionAttribute>().Destination
                select new KeyValuePair<string, string>(props.Name, destination));
        }

        public string GetDestination(string sectionKey)
        {
            return this.DestinationMappings.ContainsKey(sectionKey) ? this.DestinationMappings[sectionKey] : "#null";
        }
        

    }
}
