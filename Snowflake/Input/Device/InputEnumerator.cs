using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Controller;
using Snowflake.Service;

namespace Snowflake.Input.Device
{
    public abstract class InputEnumerator : Plugin, IInputEnumerator
    {
        public IDictionary<string, IControllerLayout> ControllerLayouts
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public abstract IEnumerable<IInputDevice> GetConnectedDevices();

        protected InputEnumerator(ICoreService coreInstance) : base(coreInstance)
        {

        }
    }
}
