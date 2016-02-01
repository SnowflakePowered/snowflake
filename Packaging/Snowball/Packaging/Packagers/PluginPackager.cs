using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowball.Packaging.Packagers
{
    public class PluginPackager : Packager
    {
        public PluginPackager() : base("plugins", PackageType.Plugin)
        {
            
        }

        public override string Make(string inputFile, string infoFile)
        {
            if (!File.Exists(Path.Combine(inputFile)))
                throw new FileNotFoundException("Unable to find the plugin file");
            var pluginAssembly = Assembly.ReflectionOnlyLoadFrom(inputFile);
            infoFile = String.IsNullOrWhiteSpace(infoFile)
                ? PluginPackager.GetPluginStringResource("snowball.json", pluginAssembly)
                : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            return this.Make(inputFile, packageInfo);
        }

        public override string Make(string inputFile, PackageInfo packageInfo)
        {
            if (!File.Exists(Path.Combine(inputFile)))
                throw new FileNotFoundException("Unable to find the plugin file");
            var pluginAssembly = Assembly.ReflectionOnlyLoadFrom(inputFile);
            string pluginRoot = Path.GetDirectoryName(inputFile);
            string pluginName =
                     JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(
                         PluginPackager.GetPluginStringResource("plugin.json", pluginAssembly))["name"];
            string resourceRoot = Path.Combine(pluginRoot, pluginName);
            string snowballDir = Packager.CopyResourceFiles(resourceRoot, packageInfo);
            File.Copy(inputFile, Path.Combine(snowballDir, Path.GetFileName(inputFile)));
            return snowballDir;
        }


        private static Stream GetPluginResource(string resourceName, Assembly assembly)
        {
            return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.resource.{resourceName}");
        }

        private static string GetPluginStringResource(string resourceName, Assembly assembly)
        {
            try
            {
                using (Stream stream = PluginPackager.GetPluginResource(resourceName, assembly))
                using (var reader = new StreamReader(stream, Encoding.UTF8, true))
                {
                    string file = reader.ReadToEnd();
                    return file;
                }
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}
