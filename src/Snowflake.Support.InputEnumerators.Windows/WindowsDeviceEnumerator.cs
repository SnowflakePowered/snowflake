using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Multimedia;
using SharpDX.XInput;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
using Snowflake.Services;
//using DeviceType = Snowflake.Input.Device.DeviceType;

namespace Snowflake.Support.InputEnumerators.Windows
{
    public class WindowsDeviceEnumerator : IDeviceEnumerator
    {
        /// <inheritdoc/>
        public IEnumerable<IInputDevice> QueryConnectedDevices()
        {
         
            yield return new InputDevice(IDeviceEnumerator.VirtualVendorID,
                IDeviceEnumerator.PassthroughDevicePID, 
                "Passthrough", "Emulator Handled", "passthrough", IDeviceEnumerator.PassthroughInstanceGuid,
                new List<IInputDeviceInstance>() { new PassthroughDeviceInstance() });

            yield return new InputDevice(IDeviceEnumerator.VirtualVendorID,
             IDeviceEnumerator.KeyboardDevicePID, "Keyboard",
             "Keyboard and Mouse", "keyboard" ,IDeviceEnumerator.KeyboardInstanceGuid,
             new List<IInputDeviceInstance>() { new KeyboardDeviceInstance() });

            using var directInput = new DirectInput();

            int xinputIndex = 0;
            var dinputProdCount = new Dictionary<(int vid, string), int>();
            var dinputPNameCount = new Dictionary<string, int>();
            var dinputClassCount = new Dictionary<(int vid, int pid), int>();

            foreach (var device in directInput.GetDevices(DeviceClass.GameControl,
                        DeviceEnumerationFlags.AttachedOnly)
                   .Select(j => new Joystick(directInput, j.InstanceGuid)))
            {
                int pid = device.Properties.ProductId;
                int vid = device.Properties.VendorId;
                int allOrder = device.Properties.JoystickId;
                string name = device.Information.ProductName;
                string path = device.Properties.InterfacePath;
                var instances = new List<IInputDeviceInstance>();

                // keep track of orderings
                dinputProdCount.TryGetValue((vid, name), out int prodOrder);
                dinputPNameCount.TryGetValue(name, out int nameOrder);
                dinputClassCount.TryGetValue((pid, vid), out int classOrder);

                dinputProdCount[(vid, name)] = prodOrder + 1;
                dinputPNameCount[name] = nameOrder + 1;
                dinputClassCount[(pid, vid)] = classOrder + 1;

                if (path.ToLowerInvariant().Contains("ig_") && xinputIndex < 4)
                {
                    var xinput = new Controller((UserIndex)xinputIndex);
                    if (xinput.IsConnected)
                    {
                        instances.Add(new XInputDeviceInstance(xinputIndex));
                    }
                }

                var objects = device.GetObjects();
                var dinputCapabilities = GetDirectInputCapabilities(objects).ToList();
                var capabilities = dinputCapabilities
                    .ToDictionary(d => d.capability, d => (d.offset, d.rawId));

                // todo add support for mapping overrides
                instances.Add(new DirectInputDeviceInstance(allOrder, classOrder, nameOrder, prodOrder, capabilities, 
                    GenerateDefaultMapping(capabilities.Keys)));

                yield return new InputDevice(vid, pid,
                    name, name, path, device.Information.InstanceGuid, instances.AsReadOnly());
            }
            
        }

        private static readonly IDictionary<DeviceCapability, ControllerElement> _hidMappings =
           new Dictionary<DeviceCapability, ControllerElement>()
           {

                {DeviceCapability.Button0, ControllerElement.ButtonA},
                {DeviceCapability.Button1, ControllerElement.ButtonB},
                {DeviceCapability.Button2, ControllerElement.ButtonX},
                {DeviceCapability.Button3, ControllerElement.ButtonY},
                {DeviceCapability.Button4, ControllerElement.ButtonL},
                {DeviceCapability.Button5, ControllerElement.ButtonR},
                {DeviceCapability.Button6, ControllerElement.ButtonStart},
                {DeviceCapability.Button7, ControllerElement.ButtonSelect},
                {DeviceCapability.Button8, ControllerElement.ButtonClickL},
                {DeviceCapability.Button9, ControllerElement.ButtonClickR},

                {DeviceCapability.Hat0N, ControllerElement.DirectionalN},
                {DeviceCapability.Hat0E, ControllerElement.DirectionalE},
                {DeviceCapability.Hat0S, ControllerElement.DirectionalS},
                {DeviceCapability.Hat0W, ControllerElement.DirectionalW},

                {DeviceCapability.Axis1Positive, ControllerElement.AxisLeftAnalogPositiveY},
                {DeviceCapability.Axis0Negative, ControllerElement.AxisLeftAnalogNegativeX},

                {DeviceCapability.Axis1Negative, ControllerElement.AxisLeftAnalogNegativeY},
                {DeviceCapability.Axis0Positive, ControllerElement.AxisLeftAnalogPositiveX},

                {DeviceCapability.Axis4Positive, ControllerElement.AxisRightAnalogPositiveY},
                {DeviceCapability.Axis3Negative, ControllerElement.AxisRightAnalogNegativeX},
                {DeviceCapability.Axis4Negative, ControllerElement.AxisRightAnalogNegativeY},
                {DeviceCapability.Axis3Positive, ControllerElement.AxisRightAnalogPositiveX},

                {DeviceCapability.Axis2Positive, ControllerElement.TriggerLeft},
                {DeviceCapability.Axis2Negative, ControllerElement.TriggerRight}

           };

        /// <summary>
        /// Tries to generate a default mapping using the default Gamepad HID Mapping
        /// https://docs.microsoft.com/en-us/windows/win32/xinput/directinput-and-xusb-devices
        /// </summary>
        /// <param name="capabilities"></param>
        /// <returns></returns>
        private static IDeviceLayoutMapping 
            GenerateDefaultMapping(IEnumerable<DeviceCapability> capabilities)
        {
            var mappings = new Dictionary<ControllerElement, DeviceCapability>();
            foreach (var c in capabilities)
            {
                if (_hidMappings.ContainsKey(c))
                {
                    mappings[_hidMappings[c]] = c;
                }
            }
            return new DeviceLayoutMapping(mappings);
        }

        private const int InstanceNumberMax = 0xFFFF - 1;
        private const int AnyInstanceMask = 0x00FFFF00;

        private static IEnumerable<(DeviceCapability capability, int offset, int rawId)> 
            GetDirectInputCapabilities(IEnumerable<DeviceObjectInstance> objects)
        {
            foreach (var deviceObj in objects) 
            {
                int instanceNumber = deviceObj.ObjectId.InstanceNumber;

                // Raw directX button ID
                int rawId = ((int)deviceObj.ObjectId.Flags & ~AnyInstanceMask) 
                    | ((instanceNumber < 0 | instanceNumber > InstanceNumberMax) ? 0 : ((instanceNumber & 0xFFFF) << 8));

                // todo get offset 
                if (deviceObj.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.PushButton)) 
                {
                    var button = DeviceCapabilityClasses.GetButton(instanceNumber);
                    if (button != DeviceCapability.None)
                    {
                        yield return (button, deviceObj.Offset, rawId);
                    }
                }
                if (deviceObj.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.AbsoluteAxis)
                        || deviceObj.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.RelativeAxis))
                {
                    var axis = DeviceCapabilityClasses.GetAxis(instanceNumber);
                    foreach (var c in axis)
                    {
                        yield return (c, deviceObj.Offset, rawId);
                    }
                }
                if (deviceObj.ObjectId.Flags.HasFlag(DeviceObjectTypeFlags.PointOfViewController))
                {
                    var hat = DeviceCapabilityClasses.GetHat(instanceNumber);
                    foreach (var c in hat)
                    {
                        yield return (c, deviceObj.Offset, rawId);
                    }
                }
            }
            yield break;
        }
    }
}
