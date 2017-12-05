using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Snowflake.Configuration.Attributes;
using Snowflake.Utility;

namespace Snowflake.Configuration
{
    public class ConfigurationSectionDescriptor<T> : IConfigurationSectionDescriptor
        where T : class, IConfigurationSection<T>
    {
        /// <inheritdoc/>
        public string Description { get; }

        /// <inheritdoc/>
        public string DisplayName { get; }

        /// <inheritdoc/>
        public string SectionName { get; }

        /// <inheritdoc/>
        public IEnumerable<IConfigurationOptionDescriptor> Options { get; }

        /// <inheritdoc/>
        public IConfigurationOptionDescriptor this[string optionKey] => this.Options.First(o => o.OptionKey == optionKey);

        internal ConfigurationSectionDescriptor()
        {
            // todo cache descriptors
            this.Options = (from prop in typeof(T).GetProperties()
                          where prop.HasAttribute<ConfigurationOptionAttribute>()
                          let attr = prop.GetCustomAttribute<ConfigurationOptionAttribute>()
                          let name = prop.Name
                          let metadata = prop.GetCustomAttributes<CustomMetadataAttribute>()
                          select new ConfigurationOptionDescriptor(attr, metadata, name))
                          .ToImmutableList();
            var sectionMetadata = typeof(T).GetAttribute<ConfigurationSectionAttribute>() ?? new ConfigurationSectionAttribute(string.Empty, string.Empty);
            this.SectionName = sectionMetadata.SectionName;
            this.DisplayName = sectionMetadata.DisplayName;
            this.Description = sectionMetadata.Description;
        }
    }
}
