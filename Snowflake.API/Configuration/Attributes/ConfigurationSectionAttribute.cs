using System;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigurationSectionAttribute : Attribute
    {
        public const string FlagsOutputPath = "#flags";

        public string SectionName { get; }
        public string Destination { get; }
        public string Description { get; set; } = String.Empty;
        public string DisplayName { get; }

        public ConfigurationSectionAttribute(string sectionName, string displayName, string destination)
        {
            this.SectionName = sectionName;
            this.Destination = destination;
            this.DisplayName = displayName;
        }
    }
}
