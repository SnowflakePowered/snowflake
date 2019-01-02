using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Snowflake.Extensibility;
using Snowflake.Extensibility.Provisioning;
using Snowflake.Input.Controller;
using Snowflake.Services;

namespace Snowflake.Input.Device
{
    public abstract class InputEnumerator : ProvisionedPlugin, IInputEnumerator
    {
        /// <inheritdoc/>
        public IControllerLayout ControllerLayout { get; }

        /// <inheritdoc/>
        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator(IPluginProvision p)
            : base(p)
        {
            this.ControllerLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(File.ReadAllText(p.ResourceDirectory.GetFiles()
                    .First(f => f.Name == "layout.json").FullName));
        }
    }
}
