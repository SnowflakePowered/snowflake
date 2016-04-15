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
using Snowflake.Service;

namespace Snowflake.Plugin.InputEnumerators
{
    [Plugin("InputEnumerator-XInput")]
    public class XInputGamepadEnumerator : InputEnumerator
    {
        private readonly IInputManager inputManager;

        public XInputGamepadEnumerator(ICoreService coreService) : base(coreService)
        {
            this.inputManager = coreService.Get<IInputManager>();
        }

        public override IEnumerable<IInputDevice> GetConnectedDevices()
        {
            var devices = this.inputManager.GetAllDevices();
            
            return (from device in devices
                where device.XI_IsXInput
                where device.XI_IsConnected == true
                select new InputDevice(InputApi.XInput, device, this.DefaultControllerLayout) { DeviceIndex = device.XI_GamepadIndex });
        }
    }
}
