using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// Serializes input configuration
    /// </summary>
    public interface IInputSerializer
    {
        /// <summary>
        /// Serializes the specified input template.
        /// </summary>
        /// <param name="inputTemplate">The input template to serialize</param>
        /// <param name="inputMapping">The input mapping to serialize with</param>
        /// <returns>The entire input template serialized as a string</returns>
        string Serialize(IInputConfiguration inputTemplate, IDeviceInputMapping inputMapping);

        /// <summary>
        /// Serializes a controller element line using the provided input mapper.
        /// </summary>
        /// <param name="key">The key of the option</param>
        /// <param name="element">The controller element to serialize</param>
        /// <param name="inputMapping">The input mapping to serialize with</param>
        /// <returns></returns>
        string SerializeInput(string key, ControllerElement element, IDeviceInputMapping inputMapping);
    }
}
