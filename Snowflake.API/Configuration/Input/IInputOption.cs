using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Represents a input configuration option in an emulator configuration,
    /// holding a serializable <see cref="IMappedControllerElement"/>
    /// </summary>
    public interface IInputOption
    {
        /// <summary>
        /// The name of the input option as it appears in configuration.
        /// </summary>
        string OptionName { get; }
        /// <summary>
        /// The value of this input option
        /// </summary>
        IMappedControllerElement Value { get; set; }
        /// <summary>
        /// The type of this input option, whether it accepts
        /// keyboard only mappings, controller button mappings, or any type of mapping
        /// </summary>
        InputOptionType InputOptionType { get; }
        /// <summary>
        /// The target controller element; the button on the virtual controller that maps to this input option
        /// </summary>
        ControllerElement TargetElement { get; }

    }
}
