using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Input.Controller;
using Snowflake.Service;

namespace Snowflake.Input.Device
{
    public abstract class InputEnumerator : Plugin, IInputEnumerator
    {
        public IControllerLayout DefaultControllerLayout => this.ControllerLayouts["default"];
        public IDictionary<string, IControllerLayout> ControllerLayouts { get; }

        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator() : base(Path.GetTempPath())
        {
            //todo fix this?
            this.ControllerLayouts = (JsonConvert.DeserializeObject<JObject>(this.PluginProperties.Get("controllerLayouts"))
                .ToObject<IDictionary<string, ControllerLayout>>() as IDictionary<string, ControllerLayout>)?
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value as IControllerLayout);
        }
    }
}
