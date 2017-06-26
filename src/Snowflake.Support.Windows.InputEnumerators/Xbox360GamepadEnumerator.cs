using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Snowflake.Extensibility;
using Snowflake.Input;
using Snowflake.Input.Device;
using Snowflake.Services;
using Snowflake.Extensibility.Provisioned;

namespace Snowflake.Plugin.InputEnumerators
{
    [Plugin("InputEnumerator-Xbox360")]
    public class Xbox360GamepadEnumerator : InputEnumerator
    {
        private readonly IInputManager inputManager;

        public Xbox360GamepadEnumerator(IPluginProvision p, IInputManager inputManager) : base(p)
        {
            this.inputManager = inputManager;
        }

        public override IEnumerable<IInputDevice> GetConnectedDevices()
        {
            return (from device in this.inputManager.GetAllDevices()
                where device.DI_InterfacePath.IndexOf("IG_", StringComparison.OrdinalIgnoreCase) >= 0
                select new InputDevice(InputApi.DirectInput, device, this.ControllerLayout));
        }
    }
}
