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
    public abstract class ConfigurationSection : IConfigurationSection
    {
        public string SectionName { get; }
        public string DisplayName { get; }
        public string ConfigurationFileName { get; }
        public string Description { get; }

        protected ConfigurationSection(string sectionName, string displayName, string configurationFilename, string description)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
            this.ConfigurationFileName = configurationFilename;
            this.Description = description;
        }

        protected ConfigurationSection(string sectionName, string displayName, string configurationFilename)
            : this(sectionName, displayName, configurationFilename, String.Empty)
        {
        }

        public IEnumerator<IConfigurationProperty> GetEnumerator()
        {
            return (from propertyInfo in this.GetType()
                .GetRuntimeProperties()
                where propertyInfo.IsDefined(typeof (ConfigurationOptionAttribute), true)
                let metadata = propertyInfo.GetCustomAttribute<ConfigurationOptionAttribute>()
                let value = propertyInfo.GetValue(this)
                select new ConfigurationProperty(value, metadata)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
