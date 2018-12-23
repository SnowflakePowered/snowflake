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
using Snowflake.Configuration.Extensions;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Default implementation for <see cref="IConfigurationCollectionDescriptor"/>
    /// </summary>
    /// <typeparam name="T">The type of the configuration collection</typeparam>
    public class ConfigurationCollectionDescriptor<T> : IConfigurationCollectionDescriptor
        where T : class, IConfigurationCollection
    {
        /// <inheritdoc/>
        public IDictionary<string, IConfigurationFile> Outputs { get; }

        /// <inheritdoc/>
        public IEnumerable<string> SectionKeys { get; }
        private IDictionary<string, string> DestinationMappings { get; }

        internal ConfigurationCollectionDescriptor()
        {
            this.Outputs = ImmutableDictionary.CreateRange(typeof(T).GetPublicAttributes<ConfigurationFileAttribute>()
                .ToDictionary(f => f.Key, f => new ConfigurationFile(f.FileName, f.Key, new BooleanMapping(f.TrueMapping, f.FalseMapping)) as IConfigurationFile));
            var sections =
                (from props in typeof(T).GetPublicProperties()
                    where props.IsDefined(typeof(SerializableSectionAttribute))
                    select props).ToImmutableList();
            this.SectionKeys = ImmutableList.CreateRange(from props in sections select props.Name);

            this.DestinationMappings = ImmutableDictionary.CreateRange(from props in sections
                let destination = props.GetAttribute<SerializableSectionAttribute>().Destination
                select new KeyValuePair<string, string>(props.Name, destination));
        }

        /// <inheritdoc/>
        public string GetDestination(string sectionKey)
        {
            return this.DestinationMappings.ContainsKey(sectionKey) ? this.DestinationMappings[sectionKey] : "#null";
        }
    }
}
