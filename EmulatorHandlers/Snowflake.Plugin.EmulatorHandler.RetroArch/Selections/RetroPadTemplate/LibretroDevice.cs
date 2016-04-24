using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Selections.RetroPadTemplate
{
    public enum LibretroDevice
    {
        [SelectionOption("0", DisplayName = "None")]
        None,
        [SelectionOption("1", DisplayName = "RetroPad")]
        RetroPad,
        [SelectionOption("5", DisplayName = "RetroPad w/ Analog")]
        RetroPadWithAnalog
    }
}
