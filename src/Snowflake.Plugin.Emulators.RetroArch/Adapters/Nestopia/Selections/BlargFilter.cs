using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Selections
{
    public enum BlargFilter
    {
        [SelectionOption("disabled", DisplayName = "Disabled")]
        Disabled,
        [SelectionOption("composite", DisplayName = "NTSC Composite")]
        Composite,
        [SelectionOption("svideo", DisplayName = "S-Video")]
        SVideo,
        [SelectionOption("rgb", DisplayName = "RGB Output")]
        RGB,
        [SelectionOption("monochrome", DisplayName = "Monochrome")]
        Monochrome,
    }
}
