using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Selections.InputConfiguration
{
    public enum InputDriver
    {
        [SelectionOption("null", DisplayName = "No Input")]
        Null,

        [SelectionOption("dinput", DisplayName = "DirectInput")]
        DirectInput,

        [SelectionOption("sdl2", DisplayName = "SDL2")]
        SDL2,
    }
}
