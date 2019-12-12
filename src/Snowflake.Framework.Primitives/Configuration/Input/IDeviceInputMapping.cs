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
    public interface IDeviceInputMapping
    {
      
        /// <summary>
        /// The driver type for this input mapping
        /// </summary>
        InputDriverType InputDriver { get; }

        /// <summary>
        /// Gets the mapping for this device capabilitiy. If the mapping does not exist,
        /// it will try to fallback to the representation for <see cref="DeviceCapability.KeyNone"/> if the given element
        /// is a keyboard key element, or <see cref="DeviceCapability.None"/> if the given element is
        /// not a keyboard key, or if there is no mapping for <see cref="DeviceCapability.KeyNone"/> . If no mapping
        /// is found for <see cref="DeviceCapability.None"/>, then the empty string is returned.
        /// </summary>
        /// 
        /// <param name="element">The mapping for this device capability</param>
        /// <returns>The string representation of the given capability defined by this mapping.</returns>
        string this[DeviceCapability element] { get; }
    }
}
