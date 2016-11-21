using System;

namespace Snowflake.Configuration.Attributes
{
    /// <summary>
    /// Marks a configuration section interface as an input template.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class InputTemplateAttribute : ConfigurationSectionAttribute
    {      
        public InputTemplateAttribute(string sectionName)
            :base (sectionName, "input")
        {
    
        }
    }
}
