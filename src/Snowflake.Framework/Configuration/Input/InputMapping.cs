using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Extensions;
using Snowflake.Input.Device;
using Snowflake.JsonConverters;

namespace Snowflake.Configuration.Input
{
    [JsonConverter(typeof(InputMappingConverter))]
    public class InputMapping : IInputMapping
    {
        private readonly IDictionary<ControllerElement, string> elementMappings;

        /// <inheritdoc/>
        public string this[ControllerElement element]
        {
            get
            {
                if (this.elementMappings.TryGetValue(element, out string mappedValue))
                {
                    return mappedValue;
                }

                if (element.IsKeyboardKey() 
                    && this.elementMappings.TryGetValue(ControllerElement.KeyNone, out string keyNone)) {
                    return keyNone;
                }

                if (this.elementMappings.TryGetValue(ControllerElement.NoElement, out string noElement))
                {
                    return noElement;
                }

                return string.Empty;
            }
        }

        /// <inheritdoc/>
        public InputApi InputApi { get; }

        /// <inheritdoc/>
        public IEnumerable<string> DeviceLayouts { get; }

        public InputMapping(IDictionary<ControllerElement, string> elementMappings,
            InputApi inputApi, IEnumerable<string> deviceLayouts)
        {
            this.elementMappings = elementMappings;
            this.InputApi = inputApi;
            this.DeviceLayouts = deviceLayouts;
        }
    }
}
