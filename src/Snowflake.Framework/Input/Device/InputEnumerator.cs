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
using Snowflake.Services;

namespace Snowflake.Input.Device
{
    public abstract class InputEnumerator : ProvisionedPlugin, IInputEnumerator
    {
        public IControllerLayout ControllerLayout { get; }

        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator(IPluginProvision p) : base(p)
        { 
            this.ControllerLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(File.ReadAllText(p.ContentDirectory.GetFiles()
                            .First(f => f.Name == "layout.json").FullName));
        }
    }
}
