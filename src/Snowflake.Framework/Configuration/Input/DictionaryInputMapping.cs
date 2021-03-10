using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// A JSON deseriazable input mapping backed by a simple dictionary lookup.
    /// </summary>
    [JsonConverter(typeof(JsonInputMappingConverter))]
    public class DictionaryInputMapping : IDeviceInputMapping
    {
        private readonly IDictionary<DeviceCapability, string> elementMappings;

        public InputDriver Driver { get; }

        /// <inheritdoc/>
        public string this[DeviceCapability element]
        {
            get
            {
                if (this.elementMappings.TryGetValue(element, out string? mappedValue)
                    && mappedValue != null)
                {
                    return mappedValue;
                }

                if (this.elementMappings.TryGetValue(DeviceCapability.None, out string? noElement)
                    && noElement != null)
                {
                    return noElement;
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Instantiate an input mapping with the given dictionary of mappings.
        /// </summary>
        /// <param name="driver">The input driver used with this mapping.</param>
        /// <param name="elementMappings">The dictionary of mappings from <see cref="DeviceCapability"/> to input configuration string.</param>
        public DictionaryInputMapping(InputDriver driver, IDictionary<DeviceCapability, string> elementMappings)
        {
            this.Driver = driver;
            this.elementMappings = elementMappings;
        }
    }
}
