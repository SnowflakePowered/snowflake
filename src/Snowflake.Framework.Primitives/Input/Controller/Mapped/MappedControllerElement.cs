using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller.Mapped
{
    /// <summary>
    /// Represents a real device controller element mapped onto a virtual device element
    /// </summary>
    public struct MappedControllerElement
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
        public MappedControllerElement(ControllerElement virtualElement, DeviceCapability deviceElement)
        {
            this.LayoutElement = virtualElement;
            this.DeviceCapability = deviceElement;
        }

        /// <summary>
        /// Create a new mapping between a virtual controller element and device capability from a key value pair
        /// </summary>
        /// <param name="kvp">The key value pair to convert</param>
        public MappedControllerElement(KeyValuePair<ControllerElement, DeviceCapability> kvp)
        {
            this.LayoutElement = kvp.Key;
            this.DeviceCapability = kvp.Value;
        }


        /// <summary>
        /// Converts between a <see cref="MappedControllerElement"/> and a <see cref="KeyValuePair{TKey, TValue}"/>
        /// of the proper type.
        /// </summary>
        /// <param name="kvp">The key value pair</param>
        public static explicit operator MappedControllerElement(KeyValuePair<ControllerElement, DeviceCapability> kvp)
        {
            return new MappedControllerElement(kvp);
        }
    }
}
