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
    /// Because having properties for each is ambiguous, simply
    /// iterate over every element or use Linq to get the equivalent element.
    /// </summary>
    public interface IMappedControllerElementCollection : IEnumerable<IMappedControllerElement>
    {
        /// <summary>
        /// Gets the device id of the real device
        /// </summary>
        string DeviceId { get; }

        /// <summary>
        /// Gets the controller id of the virtual controller layout
        /// </summary>
        string ControllerId { get; }
    }
}
