using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Emulators.RetroArch.Shaders
{
    /// <summary>
    /// Represents metadata about an exposed shader preset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class ShaderAttribute : Attribute
    {
        /// <summary>
        /// Gets this shader's name
        /// </summary>
        public string ShaderName { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this shader supportes Cg format
        /// </summary>
        public bool CgSupport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this shader supports Glsl format
        /// </summary>
        public bool GlslSupport { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this shader supports Slang format
        /// </summary>
        public bool SlangSupport { get; set; }
        public ShaderAttribute(string shaderName)
        {
            this.ShaderName = shaderName;
        }
    }
}
