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
        public IEnumerable<IConfigurationOption> Options { get; }

        public IConfigurationOption this[string optionKey] => this.Options.First(o => o.KeyName == optionKey);

        internal ConfigurationSectionDescriptor()
        {
            //todo cache descriptors
            this.Options = (from prop in typeof(T).GetProperties()
                          where prop.HasAttribute<ConfigurationOptionAttribute>()
                          let attr = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
                          let name = prop.Name
                          let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
                          select new ConfigurationOption(attr, metadata, name))
                          .ToImmutableList();
            var sectionMetadata = typeof(T).GetAttribute<ConfigurationSectionAttribute>() ?? new ConfigurationSectionAttribute("", "");
            this.SectionName = sectionMetadata.SectionName;
            this.DisplayName = sectionMetadata.DisplayName;
            this.Description = sectionMetadata.Description;
        }
    }
}
