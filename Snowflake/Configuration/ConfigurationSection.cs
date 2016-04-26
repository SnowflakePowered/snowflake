using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationSection : IConfigurationSection
    {
        public string SectionName { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public IReadOnlyDictionary<string, IConfigurationOption> Options { get; }

        protected ConfigurationSection(string sectionName, string displayName, string description)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
            this.Description = description;
            //cache the configuration properties of this section
            this.Options = this.GetConfigurationProperties();
        }

        private IReadOnlyDictionary<string, IConfigurationOption> GetConfigurationProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                where propertyInfo.IsDefined(typeof (ConfigurationOptionAttribute), true)
                let option = new ConfigurationOption(propertyInfo, this) as IConfigurationOption
                select option)
                .ToDictionary(option => option.KeyName, option => option);
        }

      
        protected ConfigurationSection(string sectionName, string displayName)
            : this(sectionName, displayName, String.Empty)
        {
        }
    }
}
