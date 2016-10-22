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

        [InputOption("input_player{N}_turbo", InputOptionType.Keyboard, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurbo { get; private set; }

        [InputOption("input_player{N}_turbo_btn", InputOptionType.Controller, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurboBtn { get; private set; }

        [InputOption("input_player{N}_turbo_axis", InputOptionType.ControllerAxes, ControllerElement.NoElement)]
        public IMappedControllerElement InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", InputOptionType.Keyboard, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerB { get; private set; }

        [InputOption("input_player{N}_b_btn", InputOptionType.Controller, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBBtn { get; private set; }

        [InputOption("input_player{N}_b_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonB)]
        public IMappedControllerElement InputPlayerBAxis { get; private set; }

        [InputOption("input_player{N}_y", InputOptionType.Keyboard, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerY { get; private set; }

        [InputOption("input_player{N}_y_btn", InputOptionType.Controller, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYBtn { get; private set; }

        [InputOption("input_player{N}_y_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonY)]
        public IMappedControllerElement InputPlayerYAxis { get; private set; }

        [InputOption("input_player{N}_select", InputOptionType.Keyboard, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelect { get; private set; }

        [InputOption("input_player{N}_select_btn", InputOptionType.Controller, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectBtn { get; private set; }

        [InputOption("input_player{N}_select_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonSelect)]
        public IMappedControllerElement InputPlayerSelectAxis { get; private set; }

        [InputOption("input_player{N}_start", InputOptionType.Keyboard, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStart { get; private set; }

        [InputOption("input_player{N}_start_btn", InputOptionType.Controller, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartBtn { get; private set; }

        [InputOption("input_player{N}_start_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonStart)]
        public IMappedControllerElement InputPlayerStartAxis { get; private set; }

        [InputOption("input_player{N}_up", InputOptionType.Keyboard, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUp { get; private set; }

        [InputOption("input_player{N}_up_btn", InputOptionType.Controller, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpBtn { get; private set; }

        [InputOption("input_player{N}_up_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalN)]
        public IMappedControllerElement InputPlayerUpAxis { get; private set; }

        [InputOption("input_player{N}_down", InputOptionType.Keyboard, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDown { get; private set; }

        [InputOption("input_player{N}_down_btn", InputOptionType.Controller, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownBtn { get; private set; }

        [InputOption("input_player{N}_down_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalS)]
        public IMappedControllerElement InputPlayerDownAxis { get; private set; }

        [InputOption("input_player{N}_left", InputOptionType.Keyboard, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeft { get; private set; }

        [InputOption("input_player{N}_left_btn", InputOptionType.Controller, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftBtn { get; private set; }

        [InputOption("input_player{N}_left_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalW)]
        public IMappedControllerElement InputPlayerLeftAxis { get; private set; }

        [InputOption("input_player{N}_right", InputOptionType.Keyboard, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRight { get; private set; }

        [InputOption("input_player{N}_right_btn", InputOptionType.Controller, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightBtn { get; private set; }

        [InputOption("input_player{N}_right_axis", InputOptionType.ControllerAxes, ControllerElement.DirectionalE)]
        public IMappedControllerElement InputPlayerRightAxis { get; private set; }

        [InputOption("input_player{N}_a", InputOptionType.Keyboard, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerA { get; private set; }

        [InputOption("input_player{N}_a_btn", InputOptionType.Controller, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerABtn { get; private set; }

        [InputOption("input_player{N}_a_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonA)]
        public IMappedControllerElement InputPlayerAAxis { get; private set; }

        [InputOption("input_player{N}_x", InputOptionType.Keyboard, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerX { get; private set; }

        [InputOption("input_player{N}_x_btn", InputOptionType.Controller, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXBtn { get; private set; }

        [InputOption("input_player{N}_x_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonX)]
        public IMappedControllerElement InputPlayerXAxis { get; private set; }

        [InputOption("input_player{N}_l", InputOptionType.Keyboard, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerL { get; private set; }

        [InputOption("input_player{N}_l_btn", InputOptionType.Controller, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLBtn { get; private set; }

        [InputOption("input_player{N}_l_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonL)]
        public IMappedControllerElement InputPlayerLAxis { get; private set; }

        [InputOption("input_player{N}_r", InputOptionType.Keyboard, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerR { get; private set; }

        [InputOption("input_player{N}_r_btn", InputOptionType.Controller, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRBtn { get; private set; }

        [InputOption("input_player{N}_r_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonR)]
        public IMappedControllerElement InputPlayerRAxis { get; private set; }

        [InputOption("input_player{N}_l2", InputOptionType.Keyboard, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2 { get; private set; }

        [InputOption("input_player{N}_l2_btn", InputOptionType.Controller, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Btn { get; private set; }

        [InputOption("input_player{N}_l2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerLeft)]
        public IMappedControllerElement InputPlayerL2Axis { get; private set; }

        [InputOption("input_player{N}_r2", InputOptionType.Keyboard, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2 { get; private set; }

        [InputOption("input_player{N}_r2_btn", InputOptionType.Controller, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Btn { get; private set; }

        [InputOption("input_player{N}_r2_axis", InputOptionType.ControllerAxes, ControllerElement.TriggerRight)]
        public IMappedControllerElement InputPlayerR2Axis { get; private set; }

        [InputOption("input_player{N}_l3", InputOptionType.Keyboard, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3 { get; private set; }

        [InputOption("input_player{N}_l3_btn", InputOptionType.Controller, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Btn { get; private set; }

        [InputOption("input_player{N}_l3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickL)]
        public IMappedControllerElement InputPlayerL3Axis { get; private set; }

        [InputOption("input_player{N}_r3", InputOptionType.Keyboard, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3 { get; private set; }

        [InputOption("input_player{N}_r3_btn", InputOptionType.Controller, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Btn { get; private set; }

        [InputOption("input_player{N}_r3_axis", InputOptionType.ControllerAxes, ControllerElement.ButtonClickR)]
        public IMappedControllerElement InputPlayerR3Axis { get; private set; }

        [InputOption("input_player{N}_l_x_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlus { get; private set; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionType.Controller, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusBtn { get; private set; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisLeftAnalogPositiveX)]
        public IMappedControllerElement InputPlayerLXPlusAxis { get; private set; }

        [InputOption("input_player{N}_l_x_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinus { get; private set; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionType.Controller, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusBtn { get; private set; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisLeftAnalogNegativeX)]
        public IMappedControllerElement InputPlayerLXMinusAxis { get; private set; }

        [InputOption("input_player{N}_l_y_plus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlus { get; private set; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionType.Controller, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusBtn { get; private set; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisLeftAnalogPositiveY)]
        public IMappedControllerElement InputPlayerLYPlusAxis { get; private set; }

        [InputOption("input_player{N}_l_y_minus", InputOptionType.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinus { get; private set; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionType.Controller, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusBtn { get; private set; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisLeftAnalogNegativeY)]
        public IMappedControllerElement InputPlayerLYMinusAxis { get; private set; }

        [InputOption("input_player{N}_r_x_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlus { get; private set; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionType.Controller, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusBtn { get; private set; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisRightAnalogPositiveX)]
        public IMappedControllerElement InputPlayerRXPlusAxis { get; private set; }

        [InputOption("input_player{N}_r_x_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinus { get; private set; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionType.Controller, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusBtn { get; private set; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisRightAnalogNegativeX)]
        public IMappedControllerElement InputPlayerRXMinusAxis { get; private set; }

        [InputOption("input_player{N}_r_y_plus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlus { get; private set; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionType.Controller, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusBtn { get; private set; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisRightAnalogPositiveY)]
        public IMappedControllerElement InputPlayerRYPlusAxis { get; private set; }

        [InputOption("input_player{N}_r_y_minus", InputOptionType.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinus { get; private set; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionType.Controller, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusBtn { get; private set; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionType.ControllerAxes, ControllerElement.AxisRightAnalogNegativeY)]
        public IMappedControllerElement InputPlayerRYMinusAxis { get; private set; }

    }
}
