using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller
{
    /// <summary>
    /// Represents a real device controller element mapped onto a virtual device element
    /// </summary>
    public struct ControllerElementMapping
    {
        /// <summary>
        /// Gets the virtual element.
        /// </summary>
        public ControllerElement LayoutElement { get; }

        /// <summary>
        /// Gets or sets the real element.
        /// </summary>
        public DeviceCapability DeviceCapability { get; set; }

        /// <summary>
        /// Create a new mapping between a virtual controller element and device capability
        /// </summary>
        /// <param name="virtualElement">The virtual controller element to map to.</param>
        /// <param name="deviceElement">The real device capability.</param>
        public ControllerElementMapping(ControllerElement virtualElement, DeviceCapability deviceElement)
            => (LayoutElement, DeviceCapability) = (virtualElement, deviceElement);

        /// <summary>
        /// Create a new mapping between a virtual controller element and device capability from a key value pair
        /// </summary>
        /// <param name="kvp">The key value pair to convert</param>
        public ControllerElementMapping(KeyValuePair<ControllerElement, DeviceCapability> kvp)
            => (LayoutElement, DeviceCapability) = (kvp.Key, kvp.Value);

        /// <summary>
        /// Converts between a <see cref="ControllerElementMapping"/> and a <see cref="KeyValuePair{TKey, TValue}"/>
        /// of the proper type.
        /// </summary>
        /// <param name="kvp">The key value pair</param>
        public static explicit operator ControllerElementMapping(KeyValuePair<ControllerElement, DeviceCapability> kvp)
        {
            return new ControllerElementMapping(kvp);
        }
    }
}
