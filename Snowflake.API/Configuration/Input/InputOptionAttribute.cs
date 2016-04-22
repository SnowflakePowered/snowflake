using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputOptionAttribute : Attribute
    {
     
        public InputOptionAttribute(string optionName, InputOptionType inputOptionType, ControllerElement targetElement)
        {
            this.OptionName = optionName;
            this.InputOptionType = inputOptionType;
            this.TargetElement = targetElement;
        }

        /// <summary>
        /// The type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        public InputOptionType InputOptionType { get; }
        /// <summary>
        /// The target controller element; the button on the virtual controller that maps to this input option
        /// </summary>
        public ControllerElement TargetElement { get; }
        /// <summary>
        /// The name of the option as it appears in configuration
        /// </summary>
        public string OptionName { get; }
    }
}
