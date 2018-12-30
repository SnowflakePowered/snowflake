using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Input.Controller.Mapped
{
    /// <summary>
    /// Represents a collection of mapped elements.
    /// </summary>
    public interface IControllerElementMappings : IEnumerable<IMappedControllerElement>
    {
        /// <summary>
        /// Gets the device id of the real device
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Gets the controller id of the virtual controller layout
        /// </summary>
        string ControllerId { get; }

        /// <summary>
        /// Gets or sets the device element mapped to the given layout element,
        /// if the layout element exists for this particular layout.
        /// 
        /// If it does not exist, then the setter does nothing, and
        /// the device element returns <see cref="ControllerElement.NoElement"/>
        /// </summary>
        /// <param name="layoutElement">The layout element</param>
        /// <returns>The device element that is mapped to the given layout element.</returns>
        ControllerElement this[ControllerElement layoutElement] { get; set; }
    }
}
