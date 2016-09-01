using System;
using System.Linq;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Plugin.EmulatorAdapter.RetroArch.Selections.RetroPadTemplate;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Input
{
    public partial class RetroPadTemplate : InputTemplate
    {
        public RetroPadTemplate() : base("input")
        {
            
        }

        [ConfigurationOption("input_device_p{N}")]
        public int InputDevice { get; set; }

        [ConfigurationOption("input_player{N}_joypad_index")]
        public int InputJoypadIndex { get; set; }

        [ConfigurationOption("input_libretro_device_p{N}", DisplayName = "Device Type")]
        public LibretroDevice InputLibretroDevice { get; set; } = LibretroDevice.RetroPad;

        [ConfigurationOption("input_player{N}_analog_dpad_mode")]
        public AnalogDpadMode InputAnalogDpadMode { get; set; } = AnalogDpadMode.None;

        public override void SetInputValues(IMappedControllerElementCollection mappedElements, IInputDevice inputDevice,
            int playerIndex)
        {
            this.InputDevice = inputDevice.DeviceInfo.DI_EnumerationNumber ?? 0;//requires dinput device here.
            base.SetInputValues(mappedElements, inputDevice, playerIndex);
        }


    }

}

