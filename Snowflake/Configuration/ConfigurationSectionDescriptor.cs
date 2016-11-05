using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public class ConfigurationSectionDescriptor<T> : IConfigurationSectionDescriptor where T: class, IConfigurationSection<T>
    {
        public string Description { get; }
        public string DisplayName { get; }
        public string SectionName { get; }
        public IDictionary<string, IConfigurationOption> Options { get; }

        public ConfigurationSectionDescriptor()
        {
            //todo cache descriptors
            var options = from prop in typeof(T).GetProperties()
                          where prop.HasAttribute<ConfigurationOptionAttribute>()
                          let attr = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
                          let name = prop.Name
                          let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
                          select new KeyValuePair<string, IConfigurationOption>(name, new ConfigurationOption(attr, metadata, name));
            var sectionMetadata = typeof(T).GetAttribute<ConfigurationSectionAttribute>() ?? new ConfigurationSectionAttribute("", "");
            this.Options = ImmutableDictionary.CreateRange(options);
            this.SectionName = sectionMetadata.SectionName;
            this.DisplayName = sectionMetadata.DisplayName;
            this.Description = sectionMetadata.Description;
        }
    }
}
