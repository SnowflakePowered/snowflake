using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensibility;
using Snowflake.Input.Device;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Plugin.InputEnumerators
{
    public class InputEnumeratorsContainer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IPluginManager))]
        [ImportService(typeof(IInputManager))]
        public void Compose(IModule module, Loader.IServiceRepository coreInstance)
        {
            var pm = coreInstance.Get<IPluginManager>();
            var im = coreInstance.Get<IInputManager>();

           // pm.Register<IInputEnumerator>(new WiimoteEnumerator(coreInstance));
            pm.Register<IInputEnumerator>(new KeyboardEnumerator(pm.GetProvision<KeyboardEnumerator>(module), im));
            pm.Register<IInputEnumerator>(new Xbox360GamepadEnumerator(pm.GetProvision<Xbox360GamepadEnumerator>(module), im));
            pm.Register<IInputEnumerator>(new XInputGamepadEnumerator(pm.GetProvision<XInputGamepadEnumerator>(module), im));
        }
    }
}
