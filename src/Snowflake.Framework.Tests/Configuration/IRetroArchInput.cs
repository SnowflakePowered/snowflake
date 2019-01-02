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
        ControllerElement InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", InputOptionType.Controller, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", InputOptionType.ControllerAxes, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", InputOptionType.Keyboard, ControllerElement.ButtonB)]
        ControllerElement InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", InputOptionType.Controller, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", InputOptionType.Keyboard, ControllerElement.ButtonY)]
        ControllerElement InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", InputOptionType.Controller, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", InputOptionType.Keyboard, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", InputOptionType.Controller, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", InputOptionType.Keyboard, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", InputOptionType.Controller, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", InputOptionType.Keyboard, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", InputOptionType.Controller, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", InputOptionType.Keyboard, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", InputOptionType.Controller, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", InputOptionType.Keyboard, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", InputOptionType.Controller, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", InputOptionType.Keyboard, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", InputOptionType.Controller, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", InputOptionType.Keyboard, ControllerElement.ButtonA)]
        ControllerElement InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", InputOptionType.Controller, ControllerElement.ButtonA)]
        ControllerElement InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonA)]
        ControllerElement InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", InputOptionType.Keyboard, ControllerElement.ButtonX)]
        ControllerElement InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", InputOptionType.Controller, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", InputOptionType.Keyboard, ControllerElement.ButtonL)]
        ControllerElement InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", InputOptionType.Controller, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", InputOptionType.Keyboard, ControllerElement.ButtonR)]
        ControllerElement InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", InputOptionType.Controller, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", InputOptionType.Keyboard, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", InputOptionType.Controller, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", InputOptionType.Keyboard, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", InputOptionType.Controller, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", InputOptionType.Keyboard, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", InputOptionType.Controller, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", InputOptionType.Keyboard, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", InputOptionType.Controller, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionType.Controller,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusAxis { get; }
    }
}
