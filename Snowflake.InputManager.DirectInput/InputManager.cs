using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SharpDX;
using SharpDX.Multimedia;
using SharpDX.DirectInput;
using SharpDX.XInput;
using Snowflake.Input;
using DeviceType = Snowflake.Input.DeviceType; 
namespace Snowflake.InputManager
{
    public class InputManager : IInputManager
    {

        public IEnumerable<IInputDevice> GetAllDevices()
        {
            IList<IInputDevice> inputDevices = new List<IInputDevice>();
            var directInput = new DirectInput();
            var devices = directInput.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AttachedOnly);
                    
                //    .Where(device => device.Information.IsHumanInterfaceDevice);

            var directInputGamepads =
                this.GetGenericGamepads(devices.Where(device => device.Usage == UsageId.GenericGamepad), directInput);
            var xinputGamepads = this.GetXInputGamepads();
            var keyboards = directInput.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AllDevices).Select(keyboard => new InputDevice()
            {
                DI_InstanceGUID = keyboard.InstanceGuid,
                DI_InstanceName = keyboard.InstanceName.Trim('\0'),
                DI_ProductName = keyboard.ProductName.Trim('\0'),
                DI_ProductGUID = keyboard.ProductGuid,
                DI_DeviceType = DeviceType.Keyboard
            });

            var mice = directInput.GetDevices(DeviceClass.Pointer, DeviceEnumerationFlags.AllDevices).Select(mouse => new InputDevice()
            {
                DI_InstanceGUID = mouse.InstanceGuid,
                DI_InstanceName = mouse.InstanceName.Trim('\0'),
                DI_ProductName = mouse.ProductName.Trim('\0'),
                DI_ProductGUID = mouse.ProductGuid,
                DI_DeviceType = DeviceType.Keyboard
            });

            return directInputGamepads.Concat(xinputGamepads).Concat(keyboards).Concat(mice);
        }

        private IEnumerable<IInputDevice> GetGenericGamepads(IEnumerable<DeviceInstance> gamepads, DirectInput directInput)
        {
            var inputDevices = new List<IInputDevice>();
            for (int i = 0; i < gamepads.Count(); i++)
            {
                DeviceInstance deviceInstance = gamepads.ElementAt(i);
                Device device = new Joystick(directInput, deviceInstance.InstanceGuid);

                var inputDevice = new InputDevice()
                {
                    DI_InstanceGUID = deviceInstance.InstanceGuid,
                    DI_InstanceName = deviceInstance.InstanceName.Trim('\0'),
                    DI_ProductName = deviceInstance.ProductName.Trim('\0'),
                    DI_ProductGUID = deviceInstance.ProductGuid,
                    DI_DeviceType = DeviceType.Gamepad,
                    DI_EnumerationNumber = i
                };

                try
                {
                    inputDevice.DI_InterfacePath = device.Properties.InterfacePath.Trim('\0');
                    inputDevice.DI_JoystickID = device.Properties.JoystickId;
                    inputDevice.DI_ProductID = device.Properties.ProductId;
                    inputDevice.DI_VendorID = device.Properties.VendorId;
                }
                catch (SharpDXException)
                {
                    inputDevice.DI_JoystickID = null;
                    inputDevice.DI_InterfacePath = null;
                }
                finally
                {
                    inputDevices.Add(inputDevice);
                }
            }
            return inputDevices;
        }

        private IEnumerable<IInputDevice> GetXInputGamepads()
        {
            var inputDevices = new List<IInputDevice>();
            for (int i = 0; i < 4; i++)
            {
                var xinput = new SharpDX.XInput.Controller((UserIndex)i);
                if (!xinput.IsConnected) continue;
                var device = new InputDevice()
                {
                    DI_DeviceType = DeviceType.Gamepad,
                    XI_GamepadIndex = i,
                    XI_IsXInput = true
                };
                inputDevices.Add(device);
            }
            return inputDevices;
        }
    }
}
