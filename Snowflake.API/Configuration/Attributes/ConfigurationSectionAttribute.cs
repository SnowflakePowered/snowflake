using System;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ConfigurationSectionAttribute : Attribute
    {
        public string SectionName { get; }
        public string Description { get; set; } = String.Empty;
        public string DisplayName { get; }

        public ConfigurationSectionAttribute(string sectionName, string displayName)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
        }
    }
}
