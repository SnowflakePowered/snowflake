using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration
{
    /// <summary>
    /// A configuration serializer for an input configuration file.
    /// </summary>
    public interface IInputConfigurationSerializer : IConfigurationSerializer
    {
        /// <summary>
        /// Serializes the proper values for a mapped controller element
        /// </summary>
        /// <param name="controllerElement">The controller element to serialize for</param>
        /// <param name="inputDevice">The input device to serialize for.</param>
        /// <returns>The value of the real device's button</returns>
        string SerializeControllerElement(IMappedControllerElement controllerElement, IInputDevice inputDevice);

        /// <summary>
        /// Serializes an input configuration.
        /// </summary>
        /// <param name="inputConfiguration">The input configuration to serialize</param>
        /// <returns>The completely serialized input configuration section</returns>
        string Serialize(IInputConfigurationSection inputConfiguration);
    }
}
