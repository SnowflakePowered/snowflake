using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;
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

        [InputOption("input_player{N}_turbo", DeviceCapabilityClass.Keyboard, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurbo { get; }

        [InputOption("input_player{N}_turbo_btn", DeviceCapabilityClass.Controller, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboBtn { get; }

        [InputOption("input_player{N}_turbo_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.NoElement)]
        ControllerElement InputPlayerTurboAxis { get; set; }

        [InputOption("input_player{N}_b", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonB)]
        ControllerElement InputPlayerB { get; }

        [InputOption("input_player{N}_b_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBBtn { get; }

        [InputOption("input_player{N}_b_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonB)]
        ControllerElement InputPlayerBAxis { get; }

        [InputOption("input_player{N}_y", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonY)]
        ControllerElement InputPlayerY { get; }

        [InputOption("input_player{N}_y_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYBtn { get; }

        [InputOption("input_player{N}_y_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonY)]
        ControllerElement InputPlayerYAxis { get; }

        [InputOption("input_player{N}_select", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelect { get; }

        [InputOption("input_player{N}_select_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectBtn { get; }

        [InputOption("input_player{N}_select_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonSelect)]
        ControllerElement InputPlayerSelectAxis { get; }

        [InputOption("input_player{N}_start", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStart { get; }

        [InputOption("input_player{N}_start_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartBtn { get; }

        [InputOption("input_player{N}_start_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonStart)]
        ControllerElement InputPlayerStartAxis { get; }

        [InputOption("input_player{N}_up", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUp { get; }

        [InputOption("input_player{N}_up_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpBtn { get; }

        [InputOption("input_player{N}_up_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalN)]
        ControllerElement InputPlayerUpAxis { get; }

        [InputOption("input_player{N}_down", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDown { get; }

        [InputOption("input_player{N}_down_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownBtn { get; }

        [InputOption("input_player{N}_down_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalS)]
        ControllerElement InputPlayerDownAxis { get; }

        [InputOption("input_player{N}_left", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeft { get; }

        [InputOption("input_player{N}_left_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftBtn { get; }

        [InputOption("input_player{N}_left_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalW)]
        ControllerElement InputPlayerLeftAxis { get; }

        [InputOption("input_player{N}_right", DeviceCapabilityClass.Keyboard, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRight { get; }

        [InputOption("input_player{N}_right_btn", DeviceCapabilityClass.Controller, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightBtn { get; }

        [InputOption("input_player{N}_right_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.DirectionalE)]
        ControllerElement InputPlayerRightAxis { get; }

        [InputOption("input_player{N}_a", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonA)]
        ControllerElement InputPlayerA { get; }

        [InputOption("input_player{N}_a_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonA)]
        ControllerElement InputPlayerABtn { get; }

        [InputOption("input_player{N}_a_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonA)]
        ControllerElement InputPlayerAAxis { get; }

        [InputOption("input_player{N}_x", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonX)]
        ControllerElement InputPlayerX { get; }

        [InputOption("input_player{N}_x_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXBtn { get; }

        [InputOption("input_player{N}_x_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonX)]
        ControllerElement InputPlayerXAxis { get; }

        [InputOption("input_player{N}_l", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonL)]
        ControllerElement InputPlayerL { get; }

        [InputOption("input_player{N}_l_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLBtn { get; }

        [InputOption("input_player{N}_l_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonL)]
        ControllerElement InputPlayerLAxis { get; }

        [InputOption("input_player{N}_r", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonR)]
        ControllerElement InputPlayerR { get; }

        [InputOption("input_player{N}_r_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRBtn { get; }

        [InputOption("input_player{N}_r_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonR)]
        ControllerElement InputPlayerRAxis { get; }

        [InputOption("input_player{N}_l2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2 { get; }

        [InputOption("input_player{N}_l2_btn", DeviceCapabilityClass.Controller, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Btn { get; }

        [InputOption("input_player{N}_l2_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.TriggerLeft)]
        ControllerElement InputPlayerL2Axis { get; }

        [InputOption("input_player{N}_r2", DeviceCapabilityClass.Keyboard, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2 { get; }

        [InputOption("input_player{N}_r2_btn", DeviceCapabilityClass.Controller, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Btn { get; }

        [InputOption("input_player{N}_r2_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.TriggerRight)]
        ControllerElement InputPlayerR2Axis { get; }

        [InputOption("input_player{N}_l3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3 { get; }

        [InputOption("input_player{N}_l3_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Btn { get; }

        [InputOption("input_player{N}_l3_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonClickL)]
        ControllerElement InputPlayerL3Axis { get; }

        [InputOption("input_player{N}_r3", DeviceCapabilityClass.Keyboard, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3 { get; }

        [InputOption("input_player{N}_r3_btn", DeviceCapabilityClass.Controller, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Btn { get; }

        [InputOption("input_player{N}_r3_axis", DeviceCapabilityClass.ControllerAxes, ControllerElement.ButtonClickR)]
        ControllerElement InputPlayerR3Axis { get; }

        [InputOption("input_player{N}_l_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlus { get; }

        [InputOption("input_player{N}_l_x_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusBtn { get; }

        [InputOption("input_player{N}_l_x_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveX)]
        ControllerElement InputPlayerLXPlusAxis { get; }

        [InputOption("input_player{N}_l_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinus { get; }

        [InputOption("input_player{N}_l_x_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusBtn { get; }

        [InputOption("input_player{N}_l_x_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeX)]
        ControllerElement InputPlayerLXMinusAxis { get; }

        [InputOption("input_player{N}_l_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlus { get; }

        [InputOption("input_player{N}_l_y_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusBtn { get; }

        [InputOption("input_player{N}_l_y_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogPositiveY)]
        ControllerElement InputPlayerLYPlusAxis { get; }

        [InputOption("input_player{N}_l_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinus { get; }

        [InputOption("input_player{N}_l_y_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusBtn { get; }

        [InputOption("input_player{N}_l_y_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisLeftAnalogNegativeY)]
        ControllerElement InputPlayerLYMinusAxis { get; }

        [InputOption("input_player{N}_r_x_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlus { get; }

        [InputOption("input_player{N}_r_x_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusBtn { get; }

        [InputOption("input_player{N}_r_x_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveX)]
        ControllerElement InputPlayerRXPlusAxis { get; }

        [InputOption("input_player{N}_r_x_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinus { get; }

        [InputOption("input_player{N}_r_x_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusBtn { get; }

        [InputOption("input_player{N}_r_x_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeX)]
        ControllerElement InputPlayerRXMinusAxis { get; }

        [InputOption("input_player{N}_r_y_plus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlus { get; }

        [InputOption("input_player{N}_r_y_plus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusBtn { get; }

        [InputOption("input_player{N}_r_y_plus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogPositiveY)]
        ControllerElement InputPlayerRYPlusAxis { get; }

        [InputOption("input_player{N}_r_y_minus", DeviceCapabilityClass.Keyboard, ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinus { get; }

        [InputOption("input_player{N}_r_y_minus_btn", DeviceCapabilityClass.Controller,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusBtn { get; }

        [InputOption("input_player{N}_r_y_minus_axis", DeviceCapabilityClass.ControllerAxes,
            ControllerElement.AxisRightAnalogNegativeY)]
        ControllerElement InputPlayerRYMinusAxis { get; }
    }
}
