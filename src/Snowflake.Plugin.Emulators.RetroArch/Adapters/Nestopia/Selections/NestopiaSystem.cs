using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Selections
{
    public enum NestopiaSystem
    {
        [SelectionOption("auto", DisplayName="Automatic")]
        Auto,
        [SelectionOption("ntsc", DisplayName = "NTSC Console")]
        NTSC,
        [SelectionOption("pal", DisplayName = "PAL Console")]
        PAL,
        [SelectionOption("famicom", DisplayName = "Famicom")]
        Famicom,
        [SelectionOption("dendy", DisplayName = "Dendy")]
        Dendy,
    }
}
