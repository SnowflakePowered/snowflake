using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Selections.AudioConfiguration
{
    public enum AudioDriver
    {
        [SelectionOption("null", DisplayName = "No Audio")]
        Null,
        [SelectionOption("xaudio", DisplayName = "XAudio (Windows)")]
        XAudio,
        [SelectionOption("dsound", DisplayName = "DirectSound (Windows)")]
        DSound
    }
}
