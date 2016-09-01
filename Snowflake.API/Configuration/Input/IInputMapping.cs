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
    /// Maps controller elements onto the real key for the device.
    /// </summary>
    public interface IInputMapping
    {
        /// <summary>
        /// The input API this mapping supports. 
        /// </summary>
        InputApi InputApi { get; }
        /// <summary>
        /// The device layouts this input mapping supports within this input API.
        /// </summary>
        IEnumerable<string> DeviceLayouts { get; }
        /// <summary>
        /// Gets the mapping for this keyboard key
        /// </summary>
        /// <param name="key">The keyboard key for this mapping</param>
        /// <returns>The mapped string for this</returns>
        string this[KeyboardKey key] { get; }
        /// <summary>
        /// Gets the mapping for this controller element, or the null value if not present
        /// </summary>
        /// <param name="element">The mapping for this controller element</param>
        /// <returns></returns>
        string this[ControllerElement element] { get; }
    }
}
