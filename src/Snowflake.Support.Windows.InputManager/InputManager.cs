using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Multimedia;
using SharpDX.XInput;
using Snowflake.Input.Device;
using DeviceType = Snowflake.Input.Device.DeviceType;

namespace Snowflake.Plugin.InputManager.Win32
{
    public class InputManager : IInputManager
    {
        /// <inheritdoc/>
        public IEnumerable<ILowLevelInputDevice> GetAllDevices()
        {
            var directInput = new SharpDX.DirectInput.DirectInput();
            var devices = directInput.GetDevices(DeviceClass.All, DeviceEnumerationFlags.AttachedOnly);

            var directInputGamepads =
                this.GetGenericGamepads(devices.Where(device => device.Type == SharpDX.DirectInput.DeviceType.Gamepad
                                                                || device.Type == SharpDX.DirectInput.DeviceType
                                                                    .Joystick), directInput);
            var xinputGamepads = this.GetXInputGamepads();
            var keyboards = directInput.GetDevices(DeviceClass.Keyboard, DeviceEnumerationFlags.AllDevices)
                .Select(keyboard => new LowLevelInputDevice()
                {
                    DiscoveryApi = InputApi.DirectInput,
                    DI_InstanceGUID = keyboard.InstanceGuid,
                    DI_InstanceName = keyboard.InstanceName.Trim('\0'),
                    DI_ProductName = keyboard.ProductName.Trim('\0'),
                    DI_ProductGUID = keyboard.ProductGuid,
                    DI_DeviceType = DeviceType.Keyboard,
                });

            var mice = directInput.GetDevices(DeviceClass.Pointer,
                DeviceEnumerationFlags.AllDevices).Select(mouse => new LowLevelInputDevice()
            {
                DiscoveryApi = InputApi.DirectInput,
                DI_InstanceGUID = mouse.InstanceGuid,
                DI_InstanceName = mouse.InstanceName.Trim('\0'),
                DI_ProductName = mouse.ProductName.Trim('\0'),
                DI_ProductGUID = mouse.ProductGuid,
                DI_DeviceType = DeviceType.Mouse,
            });

            return directInputGamepads.Concat(xinputGamepads).Concat(keyboards).Concat(mice);
        }

        private IEnumerable<ILowLevelInputDevice> GetGenericGamepads(IEnumerable<DeviceInstance> gamepads,
            SharpDX.DirectInput.DirectInput directInput)
        {
            var inputDevices = new List<ILowLevelInputDevice>();
            for (int i = 0; i < gamepads.Count(); i++)
            {
                DeviceInstance deviceInstance = gamepads.ElementAt(i);
                SharpDX.DirectInput.Device device = new Joystick(directInput, deviceInstance.InstanceGuid);

                var inputDevice = new LowLevelInputDevice()
                {
                    DiscoveryApi = InputApi.DirectInput,
                    DI_InstanceGUID = deviceInstance.InstanceGuid,
                    DI_InstanceName = deviceInstance.InstanceName.Trim('\0'),
                    DI_ProductName = deviceInstance.ProductName.Trim('\0'),
                    DI_ProductGUID = deviceInstance.ProductGuid,
                    DI_DeviceType = DeviceType.Gamepad,
                    DI_EnumerationNumber = i,
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

        private IEnumerable<ILowLevelInputDevice> GetXInputGamepads()
        {
            var inputDevices = new List<ILowLevelInputDevice>();
            for (int i = 0; i < 4; i++)
            {
                var xinput = new SharpDX.XInput.Controller((UserIndex) i);
                var device = new LowLevelInputDevice()
                {
                    DiscoveryApi = InputApi.XInput,
                    DI_DeviceType = DeviceType.Gamepad,
                    XI_GamepadIndex = i,
                    XI_IsXInput = true,
                    XI_IsConnected = xinput.IsConnected,
                };
                inputDevices.Add(device);
            }

            return inputDevices;
        }
    }
}
