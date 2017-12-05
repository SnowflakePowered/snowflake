using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Selections
{
    public enum PixelAspectRatio
    {
        [SelectionOption("8:7 PAR", DisplayName="8:7 Pixel Aspect Ratio")]
        EightSeven,
        [SelectionOption("4:3", DisplayName="4:3")]
        FourThree,
    }
}
