using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Configuration.Input;
using Snowflake.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{ 
    public partial class InputTemplate
    {
        [ConfigurationOption("input_device_p{N}")]
        public int InputDevice { get; set; } = 0;

        [ConfigurationOption("input_player{N}_joypad_index")]
        public int InputJoypadIndex { get; set; } = 0;

        [ConfigurationOption("input_libretro_device_p{N}")]
        public int InputLibretroDevice { get; set; } = 1;

        //todo is enum
        [ConfigurationOption("input_player{N}_analog_dpad_mode")]
        public int InputAnalogDpadMode { get; set; } = 0;

    
    }

}

