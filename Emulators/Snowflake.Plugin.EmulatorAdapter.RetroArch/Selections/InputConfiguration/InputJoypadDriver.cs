using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Selections.InputConfiguration
{
    public enum InputJoypadDriver
    {
        [SelectionOption("null", DisplayName = "No Input")]
        Null,
        [SelectionOption("dinput", DisplayName = "DirectInput")]
        DirectInput,
        [SelectionOption("xinput", DisplayName = "XInput")]
        XInput,
        [SelectionOption("hid", DisplayName = "Human Interface Device")]
        HID,
        [SelectionOption("sdl2", DisplayName = "SDL2")]
        SDL2
    }

}
