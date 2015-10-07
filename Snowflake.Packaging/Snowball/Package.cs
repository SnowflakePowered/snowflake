using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Newtonsoft.Json;
namespace Snowflake.Packaging.Snowball
{
    public class Package
    {
        public PackageInfo PackageInfo { get; }
        public Package(PackageInfo packageInfo)
        {
            this.PackageInfo = packageInfo;
        }

        public static Package LoadDirectory(string packageRoot)
        {
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball"))) throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText(Path.Combine(packageRoot, "snowball.json")));
            return new Package(packageInfo);
        }

        public static string MakeFromPlugin(string pluginFile, string infoFile, string outputDirectory)
        {
            if (!File.Exists(Path.Combine(pluginFile))) throw new FileNotFoundException("Unable to find the plugin file");
                var pluginRoot = Path.GetDirectoryName(pluginFile);
            var pluginAssembly = Assembly.ReflectionOnlyLoadFrom(pluginFile);
            string pluginName = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(Package.GetPluginStringResource("plugin.json", pluginAssembly))["name"];
            infoFile = infoFile ?? Package.GetPluginStringResource("snowflake.json", pluginAssembly);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            Package.GetPluginStringResource("plugin.json", pluginAssembly);
            var tempDir = Package.GetTemporaryDirectory();
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", pluginName));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(pluginRoot, pluginName)), new DirectoryInfo(Path.Combine(tempDir, "snowball", pluginName)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            return Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir);
        }

        public static string MakeFromTheme(string themeRoot, string infoFile, string outputDirectory)
        {
            if (!File.Exists(Path.Combine(themeRoot, "theme.json"))) throw new FileNotFoundException("Unable to find theme.json");
            string themeName = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(Path.Combine(themeRoot, "theme.json")))["name"];
            infoFile = infoFile ?? File.ReadAllText(Path.Combine(themeRoot, "snowball.json"));
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            var tempDir = Package.GetTemporaryDirectory();
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", themeName));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(themeRoot)), new DirectoryInfo(Path.Combine(tempDir, "snowball", themeName)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            return Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir);
        }

        public static string MakeFromEmulatorDefinition(string emulatorDefinitionFile, string infoFile, string outputDirectory)
        {
            if (!File.Exists(emulatorDefinitionFile)) throw new FileNotFoundException("Unable to find emulatordef");
            string defId = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(emulatorDefinitionFile))["id"]; 
            string emulatorRoot = Path.Combine(Path.GetDirectoryName(emulatorDefinitionFile), defId);

            infoFile = infoFile ?? File.ReadAllText(Path.Combine(emulatorRoot, "snowball.json"));
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            var tempDir = Package.GetTemporaryDirectory();
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", defId));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(emulatorRoot)), new DirectoryInfo(Path.Combine(tempDir, "snowball", emulatorRoot)));
            File.Copy(emulatorDefinitionFile, Path.Combine(tempDir, Path.GetFileName(emulatorDefinitionFile)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            return Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir);
        }


        public string Pack(string outputDirectory, string packageRoot)
        {
            var tempDir = Package.GetTemporaryDirectory();
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball"))) throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            
            File.Copy(Path.Combine(packageRoot, "snowball.json"), Path.Combine(tempDir, "snowball.json"));
            string outputFilename = Path.Combine(outputDirectory,
                $"{this.PackageInfo.PackageType}!{this.PackageInfo.Name}@{this.PackageInfo.Version}.snowball"
                    .ToLowerInvariant());
            if (File.Exists(outputFilename)) File.Delete(outputFilename);
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(Path.Combine(packageRoot, "snowball"))), new DirectoryInfo(Path.Combine(Path.Combine(tempDir, "snowball"))));
            ZipFile.CreateFromDirectory(tempDir, outputFilename, CompressionLevel.NoCompression, false);
            Directory.Delete(tempDir, true);
            return Path.Combine(outputFilename);
        }
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                Package.CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private static Stream GetPluginResource(string resourceName, Assembly assembly)
        {
            return assembly.GetManifestResourceStream($"{assembly.GetName().Name}.resource.{resourceName}");
        }
        private static string GetPluginStringResource(string resourceName, Assembly assembly)
        {
            using (Stream stream = Package.GetPluginResource(resourceName, assembly))
            using (var reader = new StreamReader(stream))
            {
                string file = reader.ReadToEnd();
                return file;
            }
        }
    }
}
