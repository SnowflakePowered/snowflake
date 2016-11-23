using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.Emulators.RetroArch.Selections
{
    public enum MenuDriver
    {
        [SelectionOption("rgui")]
        RGUI,
        [SelectionOption("zarch")]
        Zarch,
        [SelectionOption("glui")]
        GLUI,
        [SelectionOption("xmb")]
        XMB,
        [SelectionOption("null")]
        Null
    }
}
