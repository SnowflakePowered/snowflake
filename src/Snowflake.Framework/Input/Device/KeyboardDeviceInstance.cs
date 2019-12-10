using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Input.Device
{
    public sealed class KeyboardDeviceInstance : IInputDriverInstance
    {
        private static readonly IDictionary<ControllerElement, DeviceCapability> DefaultKeyboardMappings =
            new Dictionary<ControllerElement, DeviceCapability>()
            {
                {ControllerElement.Button0, DeviceCapability.Key0},
                {ControllerElement.Button1, DeviceCapability.Key1},
                {ControllerElement.Button2, DeviceCapability.Key2},
                {ControllerElement.Button3, DeviceCapability.Key3},
                {ControllerElement.Button4, DeviceCapability.Key4},
                {ControllerElement.Button5, DeviceCapability.Key5},
                {ControllerElement.Button6, DeviceCapability.Key6},
                {ControllerElement.Button7, DeviceCapability.Key7},
                {ControllerElement.Button8, DeviceCapability.Key8},
                {ControllerElement.Button9, DeviceCapability.Key9},
                {ControllerElement.ButtonStart, DeviceCapability.KeySpacebar},
                {ControllerElement.ButtonSelect, DeviceCapability.KeyEnter},
                {ControllerElement.ButtonA, DeviceCapability.KeyZ},
                {ControllerElement.ButtonB, DeviceCapability.KeyX},
                {ControllerElement.ButtonX, DeviceCapability.KeyC},
                {ControllerElement.ButtonY, DeviceCapability.KeyV},
                {ControllerElement.ButtonC, DeviceCapability.KeyC},
                {ControllerElement.ButtonL, DeviceCapability.KeyQ},
                {ControllerElement.ButtonR, DeviceCapability.KeyE},
                {ControllerElement.DirectionalN, DeviceCapability.KeyUp},
                {ControllerElement.DirectionalE, DeviceCapability.KeyRight},
                {ControllerElement.DirectionalS, DeviceCapability.KeyDown},
                {ControllerElement.DirectionalW, DeviceCapability.KeyLeft},
                {ControllerElement.AxisLeftAnalogPositiveY, DeviceCapability.KeyW},
                {ControllerElement.AxisLeftAnalogNegativeX, DeviceCapability.KeyA},
                {ControllerElement.AxisLeftAnalogNegativeY, DeviceCapability.KeyS},
                {ControllerElement.AxisLeftAnalogPositiveX, DeviceCapability.KeyD},
                {ControllerElement.AxisRightAnalogPositiveY, DeviceCapability.KeyI},
                {ControllerElement.AxisRightAnalogNegativeX, DeviceCapability.KeyJ},
                {ControllerElement.AxisRightAnalogNegativeY, DeviceCapability.KeyK},
                {ControllerElement.AxisRightAnalogPositiveX, DeviceCapability.KeyL},
            };

        public KeyboardDeviceInstance()
        {
            this.Capabilities = DeviceCapabilityClasses.Keyboard
                .Concat(DeviceCapabilityClasses.Mouse)
                .Concat(new[] { DeviceCapability.Axis0, 
                    DeviceCapability.Axis0Negative, 
                    DeviceCapability.Axis0Positive })
                .ToList();

            this.DefaultLayout = new DeviceLayoutMapping(KeyboardDeviceInstance.DefaultKeyboardMappings);
        }

        public InputDriverType Driver => InputDriverType.Keyboard;
        public int EnumerationIndex => 0;

        public int ClassEnumerationIndex => 0;

        public IEnumerable<DeviceCapability> Capabilities { get; }

        public IDeviceLayoutMapping DefaultLayout { get; }

        public int UniqueNameEnumerationIndex => 0;
    }
}
