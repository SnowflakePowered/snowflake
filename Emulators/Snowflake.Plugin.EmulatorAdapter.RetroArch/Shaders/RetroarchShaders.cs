using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Shaders
{
    public enum RetroarchShaders
    {
        [SelectionOption("crt-geom", DisplayName = "Geom Curved CRT")]
        CrtGeom,
        [SelectionOption("crt-interlaced-halation", DisplayName = "Interlaced Halation CRT")]
        CrtInterlacedHalation,
        [SelectionOption("crt-lottes", DisplayName = "Lotte's CRT")]
        CrtLottes,
        [SelectionOption("crt-lottes-halation", DisplayName ="Lotte's CRT with Halation")]
        CrtLottesHalation,
        [SelectionOption("ntsc", DisplayName = "NTSC")]
        Ntsc,
        [SelectionOption("sabr-v3.0", DisplayName = "SABR V3")]
        Sabr
    }
}
