using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Maps controller elements onto the string representation of the element for the configuration.
    /// </summary>
    public interface IInputMapping
    {
        /// <summary>
        /// Gets the input API this mapping supports.
        /// </summary>
        InputApi InputApi { get; }

        /// <summary>
        /// Gets the device layouts this input mapping supports within this input API.
        /// </summary>
        IEnumerable<string> DeviceLayouts { get; }

        /// <summary>
        /// Gets the mapping for this controller element. If the mapping does not exist,
        /// it will try to fallback to the representation for <see cref="ControllerElement.KeyNone"/> if the given element
        /// is a keyboard key element, or <see cref="ControllerElement.NoElement"/> if the given element is
        /// not a keyboard key, or if there is no mapping for <see cref="ControllerElement.KeyNone"/>. If no mapping
        /// is found for <see cref="ControllerElement.NoElement"/>, then the empty string is returned.
        /// </summary>
        /// 
        /// <param name="element">The mapping for this controller element</param>
        /// <returns>The string representation of the given element defined by this mapping.</returns>
        string this[ControllerElement element] { get; }
    }
}
