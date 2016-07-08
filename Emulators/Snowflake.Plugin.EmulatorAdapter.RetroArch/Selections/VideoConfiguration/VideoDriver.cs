using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Selections.VideoConfiguration
{
    public enum VideoDriver
    {
        [SelectionOption("null", DisplayName = "No Driver")]
        Null,
        [SelectionOption("gl", DisplayName = "OpenGL")]
        OpenGL,
        [SelectionOption("d3d", DisplayName = "Direct3D")]
        Direct3D,
        [SelectionOption("sdl2", DisplayName = "SDL2")]
        SDL2
    }
}
