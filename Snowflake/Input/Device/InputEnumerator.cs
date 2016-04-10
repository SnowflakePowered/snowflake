using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Extensibility;
using Snowflake.Input.Controller;
using Snowflake.Service;

namespace Snowflake.Input.Device
{
    public abstract class InputEnumerator : Plugin, IInputEnumerator
    {
        public IControllerLayout ControllerLayout { get; }

        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator(ICoreService coreInstance) : base(coreInstance)
        {
            this.ControllerLayout = this.PluginInfo["controllerLayout"].ToObject<ControllerLayout>();
        }
    }
}
