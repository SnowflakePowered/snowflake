using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.JsonConverters
{
    public class InputMappingConverter : JsonCreationConverter<IInputMapping>
    {
        protected override IInputMapping Create(Type objectType, JObject jObject)
        {
            IDictionary<ControllerElement, string> controllerElements = (from prop in
                jObject.Value<JObject>("Controller").Properties().Concat(jObject.Value<JObject>("Keyboard").Properties())
                select new
                {
                    element = (ControllerElement) Enum.Parse(typeof(ControllerElement), prop.Name),
                    value = prop.Value.Value<string>()
                }).ToDictionary(o => o.element, o => o.value);

         
            IEnumerable<string> deviceLayouts = jObject.Value<JArray>("DeviceLayouts").Values<string>();
            InputApi inputApi = (InputApi)Enum.Parse(typeof(InputApi), jObject.Value<string>("InputApi"));
            string nullMapping = jObject.Value<string>("NullMapping");
            return new InputMapping(controllerElements, nullMapping, inputApi, deviceLayouts);
        }
    }
}
