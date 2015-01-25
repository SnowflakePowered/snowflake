using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input.InputManager;
using SharpDX.DirectInput;

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

                var info = new InputDevice();
                info.DeviceIndex = i;
                info.DI_InstanceGUID = gamepadDevice.Information.InstanceGuid.ToString();
                info.DI_InstanceName = gamepadDevice.Information.InstanceName;
                info.DI_InterfacePath = gamepadDevice.Properties.InterfacePath;
                info.DI_ProductGUID = gamepadDevice.Information.ProductGuid.ToString();
                info.DI_ProductID = gamepadDevice.Properties.ProductId;
                info.DI_JoystickID = gamepadDevice.Properties.JoystickId;
                info.DI_ProductName = gamepadDevice.Information.ProductName;

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

    public static class Extensions
    {
        public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)
        {
            var i = 0;
            foreach (var e in ie) action(e, i++);
        }
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }
    }
}
