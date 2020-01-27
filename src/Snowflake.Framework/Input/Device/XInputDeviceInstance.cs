using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Input.Device
{
    public sealed class XInputDeviceInstance : IInputDeviceInstance
    {
        private static readonly IDictionary<ControllerElement, DeviceCapability> _xInputMappings =
           new Dictionary<ControllerElement, DeviceCapability>()
           {
                // Buttons
                {ControllerElement.ButtonStart, DeviceCapability.Button6},
                {ControllerElement.ButtonSelect, DeviceCapability.Button7},
                {ControllerElement.ButtonA, DeviceCapability.Button0},
                {ControllerElement.ButtonB, DeviceCapability.Button1},
                {ControllerElement.ButtonX, DeviceCapability.Button2},
                {ControllerElement.ButtonY, DeviceCapability.Button3},
                {ControllerElement.ButtonL, DeviceCapability.Button4},
                {ControllerElement.ButtonR, DeviceCapability.Button5},
                {ControllerElement.ButtonClickL, DeviceCapability.Button8},
                {ControllerElement.ButtonClickR, DeviceCapability.Button9},
                // Directional
                {ControllerElement.DirectionalN, DeviceCapability.Hat0N},
                {ControllerElement.DirectionalE, DeviceCapability.Hat0E},
                {ControllerElement.DirectionalS, DeviceCapability.Hat0S},
                {ControllerElement.DirectionalW, DeviceCapability.Hat0W},
                // Left Axes
                {ControllerElement.AxisLeftAnalogPositiveY, DeviceCapability.Axis1Positive},
                {ControllerElement.AxisLeftAnalogNegativeX, DeviceCapability.Axis0Negative},
                {ControllerElement.AxisLeftAnalogNegativeY, DeviceCapability.Axis1Negative},
                {ControllerElement.AxisLeftAnalogPositiveX, DeviceCapability.Axis0Positive},
                // Right Axes
                {ControllerElement.AxisRightAnalogPositiveY, DeviceCapability.Axis3Positive},
                {ControllerElement.AxisRightAnalogNegativeX, DeviceCapability.Axis2Negative},
                {ControllerElement.AxisRightAnalogNegativeY, DeviceCapability.Axis3Negative},
                {ControllerElement.AxisRightAnalogPositiveX, DeviceCapability.Axis2Positive},
                // Triggers
                {ControllerElement.TriggerLeft, DeviceCapability.Axis4},
                {ControllerElement.TriggerRight, DeviceCapability.Axis5}
           };

        private static IReadOnlyList<DeviceCapability> _xInputCapabilities = new List<DeviceCapability> {
            DeviceCapability.Button0, // A
            DeviceCapability.Button1, // B
            DeviceCapability.Button2, // X
            DeviceCapability.Button3, // Y
            DeviceCapability.Button4, // L1
            DeviceCapability.Button5, // R1
            DeviceCapability.Button6, // Back
            DeviceCapability.Button7, // Start
            DeviceCapability.Button8, // L3
            DeviceCapability.Button9, // R3

            DeviceCapability.Hat0N,
            DeviceCapability.Hat0S,
            DeviceCapability.Hat0E,
            DeviceCapability.Hat0W,

            // Left Stick X
            DeviceCapability.Axis0,
            DeviceCapability.Axis0Negative,
            DeviceCapability.Axis0Positive,

            // Left Stick Y
            DeviceCapability.Axis1,
            DeviceCapability.Axis1Negative,
            DeviceCapability.Axis1Positive,

            // Right Stick X
            DeviceCapability.Axis2,
            DeviceCapability.Axis2Negative,
            DeviceCapability.Axis2Positive,

            // Right Stick Y
            DeviceCapability.Axis3,
            DeviceCapability.Axis3Negative,
            DeviceCapability.Axis3Positive,

            // Left Trigger
            DeviceCapability.Axis4,

            // Right trigger
            DeviceCapability.Axis5,
        };

        public XInputDeviceInstance(int enumerationIndex)
        {
            if (enumerationIndex > 3) throw new ArgumentOutOfRangeException(nameof(enumerationIndex), 
                "XInput can only have at most 4 devices enumerated.");
            this.EnumerationIndex = enumerationIndex;
            this.DefaultLayout = new DeviceLayoutMapping(_xInputMappings);
        }

        public InputDriverType Driver => InputDriverType.XInput;

        public int EnumerationIndex { get; }

        public int ClassEnumerationIndex => this.EnumerationIndex;

        public int NameEnumerationIndex => this.EnumerationIndex;

        public IEnumerable<DeviceCapability> Capabilities => XInputDeviceInstance._xInputCapabilities;

        public IDeviceLayoutMapping DefaultLayout { get; }

        public int ProductEnumerationIndex => this.EnumerationIndex;
    }
}
