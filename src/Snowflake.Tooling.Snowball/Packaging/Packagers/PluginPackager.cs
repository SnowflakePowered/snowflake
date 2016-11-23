﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Loader;

namespace Snowball.Packaging.Packagers
{
    public class PluginPackager : Packager
    {
        public PluginPackager() : base("plugins", PackageType.Plugin)
        {
            
        }

        public override string Make(string themeFolder, string infoFile)
        {
            if (!File.Exists(Path.Combine(themeFolder)))
                throw new FileNotFoundException("Unable to find the plugin file");
           
            var pluginAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(themeFolder);
            infoFile = String.IsNullOrWhiteSpace(infoFile)
                ? PluginPackager.GetPluginStringResource("snowball.json", pluginAssembly)
                : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            return this.Make(themeFolder, packageInfo);
        }

        public override string Make(string themeFolder, PackageInfo packageInfo)
        {
            if (!File.Exists(Path.Combine(themeFolder)))
                throw new FileNotFoundException("Unable to find the plugin file");
            var pluginAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(themeFolder);
            string pluginRoot = Path.GetDirectoryName(themeFolder);
            string pluginName =
                     JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(
                         PluginPackager.GetPluginStringResource("plugin.json", pluginAssembly))["name"];
            string resourceRoot = Path.Combine(pluginRoot, pluginName);
            string snowballDir = Packager.CopyResourceFiles(resourceRoot, packageInfo);
            File.Copy(themeFolder, Path.Combine(snowballDir, Path.GetFileName(themeFolder)));
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
