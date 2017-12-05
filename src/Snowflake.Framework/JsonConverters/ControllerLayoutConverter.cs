using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnumsNET;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;

namespace Snowflake.JsonConverters
{
    public class ControllerLayoutConverter : JsonCreationConverter<IControllerLayout>
    {
        /// <inheritdoc/>
        protected override IControllerLayout Create(Type objectType, JObject jObject)
        {
            string layoutName = jObject.Value<string>("LayoutID");
            string friendlyName = jObject.Value<string>("FriendlyName");
            IEnumerable<string> platformsWhitelist = jObject.Value<JArray>("Platforms").Values<string>();
            var jlayout = jObject.Value<JObject>("Layout");
            bool isDevice = jObject.Value<bool>("IsRealDevice");

            // var layout = new ControllerLayout(layoutName, platformsWhitelist, friendlyName, isDevice);
            var layout = new ControllerElementCollection();
            foreach (var controllerElement in
                from layoutElements in jlayout.Properties()
                let elementKey = Enums.Parse<ControllerElement>(layoutElements.Name)
                let elementLabel = layoutElements.Value.Value<string>("Label")
                let elementType = Enums.Parse<ControllerElementType>(layoutElements.Value.Value<string>("Type"))
                select (elementKey: elementKey, elementInfo: new ControllerElementInfo(elementLabel, elementType)))
            {
                layout.Add(controllerElement.elementKey, controllerElement.elementInfo);
            }

            return new ControllerLayout(layoutName, platformsWhitelist, friendlyName, layout, isDevice);
        }
    }
}
