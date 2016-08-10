using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Service;
using Snowflake.Service.Manager;

namespace Snowflake.Plugin.InputEnumerators
{
    public class InputEnumeratorsContainer : IPluginContainer
    {
        public void Compose(ICoreService coreInstance)
        {
            var pm = coreInstance.Get<IPluginManager>();
            var im = coreInstance.Get<IInputManager>();
           // pm.Register<IInputEnumerator>(new WiimoteEnumerator(coreInstance));
           pm.Register<IInputEnumerator>(new KeyboardEnumerator(im));
            pm.Register<IInputEnumerator>(new Xbox360GamepadEnumerator(im));
            pm.Register<IInputEnumerator>(new XInputGamepadEnumerator(im));
        }
    }
}
