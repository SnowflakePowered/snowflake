using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX.DirectInput;
using Snowflake.Emulator.Input.InputManager;

namespace Snowflake.InputManager
{
    public class InputManager : IInputManager
    {

        public IList<IInputDevice> GetGamepads()
        {
            IList<IInputDevice> inputDevices = new List<IInputDevice>();
            var directInput = new DirectInput();
            var gamepads = directInput.GetDevices().Where(device => device.Usage == SharpDX.Multimedia.UsageId.GenericGamepad);

            int xinputCounter = 1; //hacky way to "guess" xinput position.
            gamepads.Each((device, i) =>
            {
                var gamepadDevice = new Joystick(directInput, device.InstanceGuid);

                var info = new InputDevice
                {
                    DeviceIndex = i,
                    DI_InstanceGUID = gamepadDevice.Information.InstanceGuid.ToString(),
                    DI_InstanceName = gamepadDevice.Information.InstanceName,
                    DI_InterfacePath = gamepadDevice.Properties.InterfacePath,
                    DI_ProductGUID = gamepadDevice.Information.ProductGuid.ToString(),
                    DI_ProductID = gamepadDevice.Properties.ProductId,
                    DI_JoystickID = gamepadDevice.Properties.JoystickId,
                    DI_ProductName = gamepadDevice.Information.ProductName
                };

                if (info.DI_InterfacePath.Contains("IG_", StringComparison.OrdinalIgnoreCase))
                {
                    info.XI_IsXInput = true;
                    info.XI_GamepadIndex = xinputCounter;
                    xinputCounter++;
                }
                else
                {
                    info.XI_IsXInput = false;
                }
                inputDevices.Add(info);
            });
            return inputDevices;
        }
    }

    internal static class Extensions
    {
        internal static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }
        internal static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
