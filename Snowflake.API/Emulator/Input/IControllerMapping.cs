using System.Collections.Generic;

namespace Snowflake.Emulator.Input
{
    /// <summary>
    /// The IControllerMapping maps values to an IControllerTemplate
    /// </summary>
    public interface IControllerMapping
    {
        /// <summary>
        /// String value mappings for inputs.
        /// </summary>
        IReadOnlyDictionary<string, string> InputMappings { get; }
        /// <summary>
        /// String value mappings for misc. keys, such as input device.
        /// </summary>
        IDictionary<string, string> KeyMappings { get; }
        /// <summary>
        /// The ControllerMappingType of the ControllerMapping
        /// </summary>
        ControllerMappingType MappingType { get; }
    }
}
