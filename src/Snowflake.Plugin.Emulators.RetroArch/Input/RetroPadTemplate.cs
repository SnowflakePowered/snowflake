using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Plugin.Emulators.RetroArch.Selections.RetroPadTemplate;

namespace Snowflake.Plugin.Emulators.RetroArch.Input
{
    [InputTemplate("input")]
    public interface RetroPadTemplate : IInputTemplate<RetroPadTemplate>
    {
        [ConfigurationOption("input_device_p{N}", 0)]
        int InputDevice { get; set; }

        [ConfigurationOption("input_player{N}_joypad_index", 0)]
        int InputJoypadIndex { get; set; }

        [ConfigurationOption("input_libretro_device_p{N}", LibretroDevice.RetroPad, DisplayName = "Device Type")]
        LibretroDevice InputLibretroDevice { get; set; }

        [ConfigurationOption("input_player{N}_analog_dpad_mode", AnalogDpadMode.None)]
        AnalogDpadMode InputAnalogDpadMode { get; set; }

        [InputOption("input_player{N}_turbo", InputOptionDeviceType.Keyboard, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", InputOptionDeviceType.Controller, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", InputOptionDeviceType.Keyboard, ControllerElement.ButtonB)]
        ControllerElement InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", InputOptionDeviceType.Keyboard, ControllerElement.ButtonY)]
        ControllerElement InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", InputOptionDeviceType.Keyboard, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", InputOptionDeviceType.Keyboard, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", InputOptionDeviceType.Keyboard, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", InputOptionDeviceType.Controller, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", InputOptionDeviceType.Keyboard, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", InputOptionDeviceType.Controller, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", InputOptionDeviceType.Keyboard, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", InputOptionDeviceType.Controller, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", InputOptionDeviceType.Keyboard, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", InputOptionDeviceType.Controller, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", InputOptionDeviceType.Keyboard, ControllerElement.ButtonA)]
        ControllerElement InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonA)]
        ControllerElement InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonA)]
        ControllerElement InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", InputOptionDeviceType.Keyboard, ControllerElement.ButtonX)]
        ControllerElement InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", InputOptionDeviceType.Keyboard, ControllerElement.ButtonL)]
        ControllerElement InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", InputOptionDeviceType.Keyboard, ControllerElement.ButtonR)]
        ControllerElement InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", InputOptionDeviceType.Keyboard, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", InputOptionDeviceType.Controller, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", InputOptionDeviceType.Keyboard, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", InputOptionDeviceType.Controller, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", InputOptionDeviceType.Keyboard, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", InputOptionDeviceType.Keyboard, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", InputOptionDeviceType.Controller, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", InputOptionDeviceType.ControllerAxes, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", InputOptionDeviceType.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", InputOptionDeviceType.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", InputOptionDeviceType.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", InputOptionDeviceType.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", InputOptionDeviceType.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", InputOptionDeviceType.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", InputOptionDeviceType.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", InputOptionDeviceType.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", InputOptionDeviceType.Controller,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", InputOptionDeviceType.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusAxis { get; }
    }
}
