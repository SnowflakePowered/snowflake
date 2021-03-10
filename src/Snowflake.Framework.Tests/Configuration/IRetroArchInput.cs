using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Tests
{
    [InputConfiguration("input")]
    public partial interface IRetroArchInput
    {
        [ConfigurationOption("input_device_p{N}", 0)]
        int InputDevice { get; set; }

        [ConfigurationOption("input_player{N}_joypad_index", 0)]
        int InputJoypadIndex { get; set; }

        [InputOption("input_player{N}_turbo", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboAxis { get; }

        [InputOption("input_player{N}_b", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", DeviceCapabilityClass.ControllerButton, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", DeviceCapabilityClass.ControllerAxis, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", DeviceCapabilityClass.ControllerButton,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", DeviceCapabilityClass.ControllerAxis,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusAxis { get; }
    }
}
