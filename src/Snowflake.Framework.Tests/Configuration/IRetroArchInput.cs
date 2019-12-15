using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Tests
{
    [InputTemplate("input")]
    public interface IRetroArchInput : IInputTemplate<IRetroArchInput>
    {
        [ConfigurationOption("input_device_p{N}", 0)]
        int InputDevice { get; set; }

        [ConfigurationOption("input_player{N}_joypad_index", 0)]
        int InputJoypadIndex { get; set; }

        [InputOption("input_player{N}_turbo", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", DeviceCapabilityClass.Controller, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", DeviceCapabilityClass.Controller, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", DeviceCapabilityClass.Controller, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusAxis { get; }
    }
}
