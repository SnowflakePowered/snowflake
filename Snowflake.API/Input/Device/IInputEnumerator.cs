using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Input.Controller;

namespace Snowflake.Input.Device
{
    public interface IInputEnumerator
    {
        IEnumerable<IInputDevice> GetConnectedDevices();
        IDictionary<string, IControllerLayout> ControllerLayouts { get; }
    }
}
