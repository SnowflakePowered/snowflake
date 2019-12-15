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
    [JsonConverter(typeof(InputMappingConverter))]
    public class InputMapping : IDeviceInputMapping
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

        /// <inheritdoc/>
        public InputDriverType InputDriver { get; }

        public InputMapping(IDictionary<DeviceCapability, string> elementMappings,
            InputDriverType inputDriver)
        {
            this.elementMappings = elementMappings;
            this.InputDriver = inputDriver;
        }
    }
}
