using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Loader;

namespace Snowflake.Plugin.InputEnumerators
{
    public class InputEnumeratorsContainer : IComposable
    {
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IInputManager))]
        public void Compose(IServiceContainer coreInstance)
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
