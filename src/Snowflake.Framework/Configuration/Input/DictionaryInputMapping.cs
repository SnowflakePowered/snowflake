using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.JsonConverters;

namespace Snowflake.Configuration.Input
{
    /// <summary>
    /// A JSON deseriazable input mapping backed by a simple dictionary lookup.
    /// todo: make this free from newtonsoft.json
    /// </summary>
    [JsonConverter(typeof(InputMappingConverter))]
    public class DictionaryInputMapping : IDeviceInputMapping
    {
        private readonly IDictionary<DeviceCapability, string> elementMappings;

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
        /// <param name="elementMappings">The dictionary of mappings from <see cref="DeviceCapability"/> to input configuration string.</param>
        public DictionaryInputMapping(IDictionary<DeviceCapability, string> elementMappings)
        {
            this.elementMappings = elementMappings;
        }
    }
}
