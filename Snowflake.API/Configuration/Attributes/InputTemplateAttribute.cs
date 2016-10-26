using System;

namespace Snowflake.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class InputTemplateAttribute : ConfigurationSectionAttribute
    {
        public const string InputOutputPath = "#input";
      
        public InputTemplateAttribute(string sectionName)
            :base (sectionName, "input", InputTemplateAttribute.InputOutputPath)
        {
    
        }
    }
}
