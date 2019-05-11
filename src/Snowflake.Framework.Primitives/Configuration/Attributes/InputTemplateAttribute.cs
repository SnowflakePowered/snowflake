using System;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Marks a configuration section interface as an input template.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class InputTemplateAttribute : ConfigurationSectionAttribute
    {
        /// <summary>
        /// Marks a configuration section interface as an input template.
        /// </summary>
        /// <param name="sectionName">The name of the configuration section.</param>
        public InputTemplateAttribute(string sectionName)
            : base(sectionName, "input")
        {
        }
    }
}
