using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Selections.VideoConfiguration
{
    public enum VideoRotation
    {
        [SelectionOption("0", DisplayName = "Normal")]
        Normal,
        [SelectionOption("1", DisplayName = "90 Degrees")]
        NinetyDegrees,
        [SelectionOption("2", DisplayName = "180 Degrees")]
        OneEightyDegrees,
        [SelectionOption("3", DisplayName = "270 Degrees")]
        TwoSeventyDegrees,
    }
}
