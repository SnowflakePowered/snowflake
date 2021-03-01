using System;

namespace Snowflake.Configuration
{
    /// <summary>
    /// Marks an interface as a configuration section that is serializable into valid configuration.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class ConfigurationSectionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the section as it appears in configuration
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// Gets or sets the description of the section.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets the human readable user-facing name of the section.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationSectionAttribute"/> class.
        /// Marks an interface as a configuration section that is serializable into valid configuration.
        /// </summary>
        public ConfigurationSectionAttribute(string sectionName, string displayName)
        {
            this.SectionName = sectionName;
            this.DisplayName = displayName;
        }
    }
}
