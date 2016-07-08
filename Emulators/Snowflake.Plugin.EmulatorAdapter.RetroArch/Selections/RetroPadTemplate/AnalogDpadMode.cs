using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Selections.RetroPadTemplate
{
    public enum AnalogDpadMode
    {
        [SelectionOption("0", DisplayName = "None")]
        None,
        [SelectionOption("1", DisplayName = "Left Analog")]
        LeftAnalog,
        [SelectionOption("2", DisplayName = "Right Analog")]
        RightAnalog
    }
}
