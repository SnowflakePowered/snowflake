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
        public IControllerLayout ControllerLayout { get; }

        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator() : base(Path.GetTempPath())
        { 
            this.ControllerLayout =
                JsonConvert.DeserializeObject<ControllerLayout>(this.GetStringResource("layout.json"));
        }
    }
}
