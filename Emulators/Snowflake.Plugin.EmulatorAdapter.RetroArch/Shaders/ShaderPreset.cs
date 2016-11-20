using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Emulators.RetroArch.Shaders
{
    public class ShaderPreset
    {
        public string ShaderName { get; }
        public string ShaderPath { get; }
        public ShaderType ShaderType { get; }

        public ShaderPreset(string shaderName, string shaderPath, ShaderType shaderType)
        {
            this.ShaderName = shaderName;
            this.ShaderPath = shaderPath;
            this.ShaderType = shaderType;
        }
    }
}
