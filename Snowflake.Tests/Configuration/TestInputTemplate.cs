using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;

namespace Snowflake.Configuration.Tests
{
    public class TestInputTemplate : InputTemplate
    {
        public TestInputTemplate() : base("input")
        {
            
        }
        public override void SetInputValues(IMappedControllerElementCollection mappedElements, IInputDevice inputDevice,
            int playerIndex)
        {
            this.InputDevice = inputDevice.DeviceInfo?.DI_EnumerationNumber ?? 0;//requires dinput device here.
            base.SetInputValues(mappedElements, inputDevice, playerIndex);
        }

        [ConfigurationOption("input_device_p{N}")]
        public int InputDevice { get; set; }

        [InputOption("input_player{N}_turbo", InputOptionType.KeyboardKey, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurbo { get; private set; }

        [InputOption("input_player{N}_turbo_btn", InputOptionType.ControllerElement, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurboBtn { get; private set; }

        [InputOption("input_player{N}_turbo_axis", InputOptionType.ControllerElementAxes, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", InputOptionType.KeyboardKey, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerB { get; private set; }

        [InputOption("input_player{N}_b_btn", InputOptionType.ControllerElement, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBBtn { get; private set; }

        [InputOption("input_player{N}_b_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBAxis { get; private set; }

        [InputOption("input_player{N}_y", InputOptionType.KeyboardKey, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerY { get; private set; }

        [InputOption("input_player{N}_y_btn", InputOptionType.ControllerElement, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYBtn { get; private set; }

        [InputOption("input_player{N}_y_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYAxis { get; private set; }

        [InputOption("input_player{N}_select", InputOptionType.KeyboardKey, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelect { get; private set; }

        [InputOption("input_player{N}_select_btn", InputOptionType.ControllerElement, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectBtn { get; private set; }

        [InputOption("input_player{N}_select_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectAxis { get; private set; }

        [InputOption("input_player{N}_start", InputOptionType.KeyboardKey, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStart { get; private set; }

        [InputOption("input_player{N}_start_btn", InputOptionType.ControllerElement, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartBtn { get; private set; }

        [InputOption("input_player{N}_start_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartAxis { get; private set; }

        [InputOption("input_player{N}_up", InputOptionType.KeyboardKey, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUp { get; private set; }

        [InputOption("input_player{N}_up_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpBtn { get; private set; }

        [InputOption("input_player{N}_up_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpAxis { get; private set; }

        [InputOption("input_player{N}_down", InputOptionType.KeyboardKey, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDown { get; private set; }

        [InputOption("input_player{N}_down_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownBtn { get; private set; }

        [InputOption("input_player{N}_down_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownAxis { get; private set; }

        [InputOption("input_player{N}_left", InputOptionType.KeyboardKey, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeft { get; private set; }

        [InputOption("input_player{N}_left_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftBtn { get; private set; }

        [InputOption("input_player{N}_left_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftAxis { get; private set; }

        [InputOption("input_player{N}_right", InputOptionType.KeyboardKey, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRight { get; private set; }

        [InputOption("input_player{N}_right_btn", InputOptionType.ControllerElement, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightBtn { get; private set; }

        [InputOption("input_player{N}_right_axis", InputOptionType.ControllerElementAxes, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightAxis { get; private set; }

        [InputOption("input_player{N}_a", InputOptionType.KeyboardKey, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerA { get; private set; }

        [InputOption("input_player{N}_a_btn", InputOptionType.ControllerElement, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerABtn { get; private set; }

        [InputOption("input_player{N}_a_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerAAxis { get; private set; }

        [InputOption("input_player{N}_x", InputOptionType.KeyboardKey, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerX { get; private set; }

        [InputOption("input_player{N}_x_btn", InputOptionType.ControllerElement, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXBtn { get; private set; }

        [InputOption("input_player{N}_x_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXAxis { get; private set; }

        [InputOption("input_player{N}_l", InputOptionType.KeyboardKey, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerL { get; private set; }

        [InputOption("input_player{N}_l_btn", InputOptionType.ControllerElement, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLBtn { get; private set; }

        [InputOption("input_player{N}_l_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLAxis { get; private set; }

        [InputOption("input_player{N}_r", InputOptionType.KeyboardKey, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerR { get; private set; }

        [InputOption("input_player{N}_r_btn", InputOptionType.ControllerElement, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRBtn { get; private set; }

        [InputOption("input_player{N}_r_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRAxis { get; private set; }

        [InputOption("input_player{N}_l2", InputOptionType.KeyboardKey, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2 { get; private set; }

        [InputOption("input_player{N}_l2_btn", InputOptionType.ControllerElement, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Btn { get; private set; }

        [InputOption("input_player{N}_l2_axis", InputOptionType.ControllerElementAxes, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Axis { get; private set; }

        [InputOption("input_player{N}_r2", InputOptionType.KeyboardKey, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2 { get; private set; }

        [InputOption("input_player{N}_r2_btn", InputOptionType.ControllerElement, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Btn { get; private set; }

        [InputOption("input_player{N}_r2_axis", InputOptionType.ControllerElementAxes, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Axis { get; private set; }

        [InputOption("input_player{N}_l3", InputOptionType.KeyboardKey, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3 { get; private set; }

        [InputOption("input_player{N}_l3_btn", InputOptionType.ControllerElement, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Btn { get; private set; }

        [InputOption("input_player{N}_l3_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Axis { get; private set; }

        [InputOption("input_player{N}_r3", InputOptionType.KeyboardKey, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3 { get; private set; }

        [InputOption("input_player{N}_r3_btn", InputOptionType.ControllerElement, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Btn { get; private set; }

        [InputOption("input_player{N}_r3_axis", InputOptionType.ControllerElementAxes, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Axis { get; private set; }

        [InputOption("input_player{N}_l_x_plus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlus { get; private set; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusBtn { get; private set; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusAxis { get; private set; }

        [InputOption("input_player{N}_l_x_minus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinus { get; private set; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusBtn { get; private set; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusAxis { get; private set; }

        [InputOption("input_player{N}_l_y_plus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlus { get; private set; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusBtn { get; private set; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusAxis { get; private set; }

        [InputOption("input_player{N}_l_y_minus", InputOptionType.KeyboardKey, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinus { get; private set; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusBtn { get; private set; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusAxis { get; private set; }

        [InputOption("input_player{N}_r_x_plus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlus { get; private set; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusBtn { get; private set; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusAxis { get; private set; }

        [InputOption("input_player{N}_r_x_minus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinus { get; private set; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusBtn { get; private set; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusAxis { get; private set; }

        [InputOption("input_player{N}_r_y_plus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlus { get; private set; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusBtn { get; private set; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusAxis { get; private set; }

        [InputOption("input_player{N}_r_y_minus", InputOptionType.KeyboardKey, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinus { get; private set; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionType.ControllerElement, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusBtn { get; private set; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionType.ControllerElementAxes, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusAxis { get; private set; }

    }
}
