using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Controller;

namespace Snowflake.Input.Device
{
    /// <summary>
    /// Enumerates the available devices on the system that implement a certain controller layout
    /// </summary>
    public interface IInputEnumerator : IPlugin
    {
        /// <summary>
        /// Gets the connected devices associated with this controller layout
        /// </summary>
        /// <returns>The input devices associated with this controller layout</returns>
        IEnumerable<IInputDevice> GetConnectedDevices();

        /// <summary>
        /// The controller layout associated with this input enumerator
        /// </summary>
        IControllerLayout ControllerLayout { get; }
        
    }
}
