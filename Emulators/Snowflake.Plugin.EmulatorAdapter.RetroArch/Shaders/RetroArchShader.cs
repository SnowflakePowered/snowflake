using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Attributes;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Shaders
{
    public enum RetroArchShader
    {
        [Shader("")]
        [SelectionOption("#flag", DisplayName = "No Shader")]
        None,
        [Shader("crt-geom", CgSupport = true, GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName = "CRT Geom")]
        CrtGeom,
        [Shader("crt-lottes-multipass", GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName = "CRT Lottes")]
        CrtLottes,
        [Shader("crt-lottes-multipass-interlaced-glow", GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName ="CRT Lottes with Interlacing and Glow")]
        CrtLottesInterlaceGlow,
        [Shader("crt-hylian", CgSupport = true, GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName = "CRT Hylian")]
        CrtHylian,
        [Shader("ntsc", CgSupport = true, GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName = "NTSC")]
        Ntsc,
        [Shader("sabr-v3.0", CgSupport = true, GlslSupport = true)]
        [SelectionOption("#flag", DisplayName = "SABR Upscaling")]
        Sabr,
        [Shader("advanced-aa", CgSupport = true, GlslSupport = true, SlangSupport = true)]
        [SelectionOption("#flag", DisplayName = "Advanced Anti-aliasing")]
        AdvancedAA
    }
}
