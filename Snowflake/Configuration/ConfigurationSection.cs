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
using Snowflake.Utility;

namespace Snowflake.Configuration
{
    public abstract class ConfigurationSection : IConfigurationSection
    {
        private static Guid GetSectionGuid(string fileName, string sectionName)
            => GuidUtility.Create(ConfigurationCollection.ConfigurationSectionGuid, $"{fileName}::{sectionName}");

        public string SectionName { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public Guid SectionGuid => GuidUtility.Create(ConfigurationCollection.ConfigurationSectionGuid, $"{this.ConfigFilename}::{this.SectionName}");
        [JsonIgnore]
        internal string ConfigFilename { get; set; } = String.Empty;
        public IReadOnlyDictionary<string, IConfigurationOption> Options => _options.Value;
        private Lazy<IReadOnlyDictionary<string, IConfigurationOption>> _options;
        protected ConfigurationSection(string sectionName, string displayName, string description)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
            this.Description = description;
            //cache the configuration properties of this section
            this._options = new Lazy<IReadOnlyDictionary<string, IConfigurationOption>>(this.GetConfigurationProperties);
        }

        internal IReadOnlyDictionary<string, IConfigurationOption> GetConfigurationProperties()
        {
            return (from propertyInfo in this.GetType().GetRuntimeProperties()
                where propertyInfo.IsDefined(typeof (ConfigurationOptionAttribute), true)
                let option = new ConfigurationOption(propertyInfo, this, this.SectionGuid) as IConfigurationOption
                select option).ToDictionary(option => option.KeyName, option => option);
        }


        protected ConfigurationSection(string sectionName, string displayName)
            : this(sectionName, displayName, String.Empty)
        {
        }
    }
}
