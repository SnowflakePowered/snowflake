using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Emulators.RetroArch.Shaders
{
    public class ShaderManager
    {
        private IList<ShaderPreset> ShaderList { get; }

        public ShaderManager(string shaderPath)
        {
            this.ShaderList = ShaderManager.EnumerateShaders(shaderPath).ToList();
        }

        public string GetShaderPath(string shaderName, ShaderType shaderType)
        {
            return (from preset in this.ShaderList
                where preset.ShaderName.Equals(shaderName, StringComparison.OrdinalIgnoreCase)
                where preset.ShaderType == shaderType
                select preset.ShaderPath).FirstOrDefault() ?? string.Empty;
        }

        private static IEnumerable<ShaderPreset> EnumerateShaders(string shaderPath)
        {
            return from file in Directory.EnumerateFiles(shaderPath, "*", SearchOption.AllDirectories)
                let extension = Path.GetExtension(file)
                where extension == ".glslp" || extension == ".cgp" || extension == ".slangp"
                let name = Path.GetFileNameWithoutExtension(file)
                let type = extension == ".slangp" ? ShaderType.Slang :
                extension == ".cgp" ? ShaderType.Cg :
                extension == ".glslp" ? ShaderType.Glsl :
                ShaderType.Unknown
                select new ShaderPreset(name, file, type);
        }
    }
}
