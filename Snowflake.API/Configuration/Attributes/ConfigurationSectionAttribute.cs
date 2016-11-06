using System;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Marks an interface as a configuration section that is serializable into valid configuration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// The name of the section as it appears in configuration
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// The description of the section.
        /// </summary>
        public string Description { get; set; } = String.Empty;

        /// <summary>
        /// The human readable user-facing name of the section.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Marks an interface as a configuration section that is serializable into valid configuration.
        /// </summary>
        public ConfigurationSectionAttribute(string sectionName, string displayName)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
        }
    }
}
