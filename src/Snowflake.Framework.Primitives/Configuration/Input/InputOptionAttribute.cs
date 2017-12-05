using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Marks an attribute as an input option
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InputOptionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputOptionAttribute"/> class.
        /// Marks an attribute as an input option
        /// </summary>
        public InputOptionAttribute(string optionName, InputOptionType inputOptionType, ControllerElement targetElement)
        {
            this.OptionName = optionName;
            this.InputOptionType = inputOptionType;
            this.TargetElement = targetElement;
        }

        /// <summary>
        /// Gets the type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        public InputOptionType InputOptionType { get; }

        /// <summary>
        /// Gets the target controller element; the button on the virtual controller that maps to this input option
        /// </summary>
        public ControllerElement TargetElement { get; }

        /// <summary>
        /// Gets the name of the option as it appears in configuration
        /// </summary>
        public string OptionName { get; }
    }
}
