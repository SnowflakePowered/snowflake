using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{
    public partial class InputTemplate
    {

        [InputOption("input_player{N}_turbo", InputOptionType.KeyboardKey, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurbo { get; set; }

        [InputOption("input_player{N}_turbo_btn", InputOptionType.ControllerElement, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurboBtn { get; set; }

        [InputOption("input_player{N}_b", InputOptionType.KeyboardKey, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerB { get; set; }

        [InputOption("input_player{N}_b_btn", InputOptionType.ControllerElement, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBBtn { get; set; }

        [InputOption("input_player{N}_b_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBAxis { get; set; }

        [InputOption("input_player{N}_y", InputOptionType.KeyboardKey, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerY { get; set; }

        [InputOption("input_player{N}_y_btn", InputOptionType.ControllerElement, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYBtn { get; set; }

        [InputOption("input_player{N}_y_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYAxis { get; set; }

        [InputOption("input_player{N}_select", InputOptionType.KeyboardKey, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelect { get; set; }

        [InputOption("input_player{N}_select_btn", InputOptionType.ControllerElement, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectBtn { get; set; }

        [InputOption("input_player{N}_select_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectAxis { get; set; }

        [InputOption("input_player{N}_start", InputOptionType.KeyboardKey, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStart { get; set; }

        [InputOption("input_player{N}_start_btn", InputOptionType.ControllerElement, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartBtn { get; set; }

        [InputOption("input_player{N}_start_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartAxis { get; set; }

        [InputOption("input_player{N}_up", InputOptionType.KeyboardKey, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUp { get; set; }

        [InputOption("input_player{N}_up_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpBtn { get; set; }

        [InputOption("input_player{N}_up_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpAxis { get; set; }

        [InputOption("input_player{N}_down", InputOptionType.KeyboardKey, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDown { get; set; }

        [InputOption("input_player{N}_down_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownBtn { get; set; }

        [InputOption("input_player{N}_down_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownAxis { get; set; }

        [InputOption("input_player{N}_left", InputOptionType.KeyboardKey, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeft { get; set; }

        [InputOption("input_player{N}_left_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftBtn { get; set; }

        [InputOption("input_player{N}_left_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftAxis { get; set; }

        [InputOption("input_player{N}_right", InputOptionType.KeyboardKey, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRight { get; set; }

        [InputOption("input_player{N}_right_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightBtn { get; set; }

        [InputOption("input_player{N}_right_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightAxis { get; set; }

        [InputOption("input_player{N}_a", InputOptionType.KeyboardKey, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerA { get; set; }

        [InputOption("input_player{N}_a_btn", InputOptionType.ControllerElement, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerABtn { get; set; }

        [InputOption("input_player{N}_a_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerAAxis { get; set; }

        [InputOption("input_player{N}_x", InputOptionType.KeyboardKey, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerX { get; set; }

        [InputOption("input_player{N}_x_btn", InputOptionType.ControllerElement, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXBtn { get; set; }

        [InputOption("input_player{N}_x_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXAxis { get; set; }

        [InputOption("input_player{N}_l", InputOptionType.KeyboardKey, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerL { get; set; }

        [InputOption("input_player{N}_l_btn", InputOptionType.ControllerElement, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLBtn { get; set; }

        [InputOption("input_player{N}_l_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLAxis { get; set; }

        [InputOption("input_player{N}_r", InputOptionType.KeyboardKey, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerR { get; set; }

        [InputOption("input_player{N}_r_btn", InputOptionType.ControllerElement, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRBtn { get; set; }

        [InputOption("input_player{N}_r_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRAxis { get; set; }

        [InputOption("input_player{N}_l2", InputOptionType.KeyboardKey, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2 { get; set; }

        [InputOption("input_player{N}_l2_btn", InputOptionType.ControllerElement, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Btn { get; set; }

        [InputOption("input_player{N}_l2_axis", InputOptionType.ControllerElementAxes, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Axis { get; set; }

        [InputOption("input_player{N}_r2", InputOptionType.KeyboardKey, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2 { get; set; }

        [InputOption("input_player{N}_r2_btn", InputOptionType.ControllerElement, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Btn { get; set; }

        [InputOption("input_player{N}_r2_axis", InputOptionType.ControllerElementAxes, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Axis { get; set; }

        [InputOption("input_player{N}_l3", InputOptionType.KeyboardKey, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3 { get; set; }

        [InputOption("input_player{N}_l3_btn", InputOptionType.ControllerElement, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Btn { get; set; }

        [InputOption("input_player{N}_l3_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Axis { get; set; }

        [InputOption("input_player{N}_r3", InputOptionType.KeyboardKey, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3 { get; set; }

        [InputOption("input_player{N}_r3_btn", InputOptionType.ControllerElement, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Btn { get; set; }

        [InputOption("input_player{N}_r3_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Axis { get; set; }

        [InputOption("input_player{N}_l_x_plus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlus { get; set; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusBtn { get; set; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusAxis { get; set; }

        [InputOption("input_player{N}_l_x_minus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinus { get; set; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusBtn { get; set; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusAxis { get; set; }

        [InputOption("input_player{N}_l_y_plus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlus { get; set; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusBtn { get; set; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusAxis { get; set; }

        [InputOption("input_player{N}_l_y_minus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinus { get; set; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusBtn { get; set; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusAxis { get; set; }

        [InputOption("input_player{N}_r_x_plus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlus { get; set; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusBtn { get; set; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusAxis { get; set; }

        [InputOption("input_player{N}_r_x_minus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinus { get; set; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusBtn { get; set; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusAxis { get; set; }

        [InputOption("input_player{N}_r_y_plus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlus { get; set; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusBtn { get; set; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusAxis { get; set; }

        [InputOption("input_player{N}_r_y_minus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinus { get; set; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusBtn { get; set; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusAxis { get; set; }

    }
}
