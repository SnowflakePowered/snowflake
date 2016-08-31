﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Shaders
{
    /// <summary>
    /// Represents metadata about an exposed shader preset.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class ShaderAttribute : Attribute
    {
        /// <summary>
        /// This shader's name
        /// </summary>
        public string ShaderName { get; }
        /// <summary>
        /// This shader supportes Cg format
        /// </summary>
        public bool CgSupport { get; set; }
        /// <summary>
        /// This shader supports Glsl format
        /// </summary>
        public bool GlslSupport { get; set; }
        /// <summary>
        /// This shader supports Slang format
        /// </summary>
        public bool SlangSupport { get; set; }
        public ShaderAttribute(string shaderName)
        {
            this.ShaderName = shaderName;
        }
    }
}
