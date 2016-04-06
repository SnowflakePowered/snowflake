using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Snowflake.Input.Controller;

namespace Snowflake.JsonConverters
{
    public class ControllerLayoutConverter : JsonCreationConverter<ControllerLayout>
    {
        protected override ControllerLayout Create(Type objectType, JObject jObject)
        {
            string layoutName = jObject.Value<string>("LayoutName");
            string friendlyName = jObject.Value<string>("FriendlyName");
            IEnumerable<string> platformsWhitelist = jObject.Value<JArray>("PlatformsWhitelist").ToObject<IEnumerable<string>>();
            var jlayout = jObject.Value<JObject>("Layout");
            bool isDevice = jObject.Value<bool>("IsRealDevice");

            //var layout = new ControllerLayout(layoutName, platformsWhitelist, friendlyName, isDevice);
            var layout = new ControllerElementCollection();
            foreach (var controllerElement in 
                from layoutElements in jlayout.Properties()
                let elementKey = (ControllerElement) Enum.Parse(typeof(ControllerElement), layoutElements.Name)
                let elementLabel = layoutElements.Value.Value<string>("Label")
                let elementType = (ControllerElementType)Enum.Parse(typeof(ControllerElementType),
                layoutElements.Value.Value<string>("Type"))
                select Tuple.Create(elementKey, new ControllerElementInfo(elementLabel, elementType)))
            {
                layout.Add(controllerElement.Item1, controllerElement.Item2); //todo wait for c# 7 real tuples
            }
            return new ControllerLayout(layoutName, platformsWhitelist, friendlyName, layout, isDevice);
        }
    }
}
