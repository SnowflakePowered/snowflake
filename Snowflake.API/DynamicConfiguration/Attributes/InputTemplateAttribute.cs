using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.DynamicConfiguration.Attributes
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
