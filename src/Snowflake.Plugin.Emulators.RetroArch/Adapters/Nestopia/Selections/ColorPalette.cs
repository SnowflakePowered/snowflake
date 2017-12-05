using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Selections
{
    public enum ColorPalette
    {
        [SelectionOption("consumer", DisplayName = "Consumer")]
        Consumer,
        [SelectionOption("canonical", DisplayName = "Canonical")]
        Canonical,
        [SelectionOption("alternative", DisplayName = "Alternative")]
        Alternative,
        [SelectionOption("rgb", DisplayName = "RGB Output")]
        RGB,
        [SelectionOption("yuv-v3", DisplayName = "YUV Output")]
        YUV,
        [SelectionOption("unsaturated-v6", DisplayName = "Unsaturated")]
        Unsaturated,
        [SelectionOption("pal", DisplayName = "PAL Output")]
        PAL,
        [SelectionOption("raw", DisplayName = "Raw Output")]
        Raw,
    }
}
