using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;

namespace Snowflake.Plugin.Emulators.RetroArch.Selections.InputConfiguration
{
    public enum InputPollType
    {
        [SelectionOption("0", DisplayName = "Normal Polling")]
        Normal,

        [SelectionOption("1", DisplayName = "Late Polling")]
        Late,

        [SelectionOption("2", DisplayName = "Early Polling")]
        Early,
    }
}
