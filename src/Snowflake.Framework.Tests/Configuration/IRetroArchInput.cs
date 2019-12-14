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

        [InputOption("input_player{N}_turbo", InputOptionType.Keyboard, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", InputOptionType.Controller, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", InputOptionType.ControllerAxes, ControllerElement.NoElement)]
        DeviceCapability InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", InputOptionType.Keyboard, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", InputOptionType.Controller, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonB)]
        DeviceCapability InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", InputOptionType.Keyboard, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", InputOptionType.Controller, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonY)]
        DeviceCapability InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", InputOptionType.Keyboard, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", InputOptionType.Controller, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonSelect)]
        DeviceCapability InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", InputOptionType.Keyboard, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", InputOptionType.Controller, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonStart)]
        DeviceCapability InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", InputOptionType.Keyboard, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", InputOptionType.Controller, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalN)]
        DeviceCapability InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", InputOptionType.Keyboard, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", InputOptionType.Controller, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalS)]
        DeviceCapability InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", InputOptionType.Keyboard, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", InputOptionType.Controller, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalW)]
        DeviceCapability InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", InputOptionType.Keyboard, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", InputOptionType.Controller, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalE)]
        DeviceCapability InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", InputOptionType.Keyboard, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", InputOptionType.Controller, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonA)]
        DeviceCapability InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", InputOptionType.Keyboard, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", InputOptionType.Controller, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonX)]
        DeviceCapability InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", InputOptionType.Keyboard, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", InputOptionType.Controller, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonL)]
        DeviceCapability InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", InputOptionType.Keyboard, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", InputOptionType.Controller, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonR)]
        DeviceCapability InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", InputOptionType.Keyboard, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", InputOptionType.Controller, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerLeft)]
        DeviceCapability InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", InputOptionType.Keyboard, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", InputOptionType.Controller, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerRight)]
        DeviceCapability InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", InputOptionType.Keyboard, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", InputOptionType.Controller, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickL)]
        DeviceCapability InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", InputOptionType.Keyboard, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", InputOptionType.Controller, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickR)]
        DeviceCapability InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveX)]
        DeviceCapability InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeX)]
        DeviceCapability InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveY)]
        DeviceCapability InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeY)]
        DeviceCapability InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveX)]
        DeviceCapability InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeX)]
        DeviceCapability InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveY)]
        DeviceCapability InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeY)]
        DeviceCapability InputPlayerRYMinusAxis { get; }
    }
}
