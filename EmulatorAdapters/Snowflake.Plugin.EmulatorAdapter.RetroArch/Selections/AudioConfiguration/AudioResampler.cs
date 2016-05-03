using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Selections.AudioConfiguration
{
    public enum AudioResampler
    {
        [SelectionOption("null")]
        Null,
        [SelectionOption("sinc")]
        Sinc,
        [SelectionOption("nearest")]
        Nearest,
        [SelectionOption("CC")]
        CC
    }
}
