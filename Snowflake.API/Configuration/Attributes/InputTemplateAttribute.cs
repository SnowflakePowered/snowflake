using System;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class InputTemplateAttribute : ConfigurationSectionAttribute
    {      
        public InputTemplateAttribute(string sectionName)
            :base (sectionName, "input")
        {
    
        }
    }
}
