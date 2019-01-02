using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Configuration.Tests
{
    public enum VideoDriver
    {
        [SelectionOption("null", DisplayName = "No Driver", Private = true)]
        Null,

        [SelectionOption("gl", DisplayName = "OpenGL")]
        OpenGL,

        [CustomMetadata("osRestricted", "windows")] [SelectionOption("d3d", DisplayName = "Direct3D")]
        Direct3D,

        [SelectionOption("sdl2", DisplayName = "SDL2", Private = true)]
        SDL2,

        [SelectionOption("vulkan", DisplayName = "vulkan")]
        Vulkan,
    }
}
