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

        public static Package FromZip(string zipFile)
        {
            using (ZipArchive snowball = new ZipArchive(File.Open(zipFile, FileMode.Open), ZipArchiveMode.Read))
            {
                return new Package(JsonConvert.DeserializeObject<PackageInfo>(new StreamReader(snowball.GetEntry("snowball.json").Open()).ReadToEnd()));
            }
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
            infoFile = String.IsNullOrWhiteSpace(infoFile)
                ? Package.GetPluginStringResource("snowflake.json", pluginAssembly)
                : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            Package.GetPluginStringResource("plugin.json", pluginAssembly);
            var tempDir = Package.GetTemporaryDirectory();
            Console.WriteLine(
          $"Packing {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version} to {outputDirectory}");
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", pluginName));
            File.Copy(pluginFile, Path.Combine(tempDir, "snowball", pluginFile));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(pluginRoot, pluginName)), new DirectoryInfo(Path.Combine(tempDir, "snowball", pluginName)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            string packagePath = Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir, true);
            Directory.Delete(tempDir, true);
            return packagePath;
        }

        public static string MakeFromTheme(string themeRoot, string infoFile, string outputDirectory)
        {
            if (!File.Exists(Path.Combine(themeRoot, "theme.json"))) throw new FileNotFoundException("Unable to find theme.json");
            string themeName = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(Path.Combine(themeRoot, "theme.json")))["id"];
            infoFile = String.IsNullOrWhiteSpace(infoFile) ? File.ReadAllText(Path.Combine(themeRoot, "snowball.json")) : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            var tempDir = Package.GetTemporaryDirectory();
            Console.WriteLine(
          $"Packing {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version} to {outputDirectory}");
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", themeName));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(themeRoot)), new DirectoryInfo(Path.Combine(tempDir, "snowball", themeName)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            string packagePath = Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir, true);
            Directory.Delete(tempDir, true);
            return packagePath;
        }

        public static string MakeFromEmulatorDefinition(string emulatorDefinitionFile, string infoFile, string outputDirectory)
        {
            if (!File.Exists(emulatorDefinitionFile)) throw new FileNotFoundException("Unable to find emulatordef");
            string defId = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(File.ReadAllText(emulatorDefinitionFile))["id"]; 
            string emulatorRoot = Path.Combine(Path.GetDirectoryName(emulatorDefinitionFile), defId);

            infoFile = String.IsNullOrWhiteSpace(infoFile) ? File.ReadAllText(Path.Combine(emulatorRoot, "snowball.json")) : File.ReadAllText(infoFile);
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(infoFile);
            Console.WriteLine(
              $"Packing {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version} to {outputDirectory}");
            var tempDir = Package.GetTemporaryDirectory();
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball", defId));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(emulatorRoot)), new DirectoryInfo(Path.Combine(tempDir, "snowball", emulatorRoot)));
            File.Copy(emulatorDefinitionFile, Path.Combine(tempDir, "snowball", Path.GetFileName(emulatorDefinitionFile)));
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            string packagePath = Package.LoadDirectory(tempDir).Pack(outputDirectory, tempDir, true);
            Directory.Delete(tempDir, true);
            return packagePath;
        }

        public string Pack(string outputDirectory, string packageRoot, bool nocopy = false)
        {
            var tempDir = Package.GetTemporaryDirectory();
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball"))) throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            
            File.Copy(Path.Combine(packageRoot, "snowball.json"), Path.Combine(tempDir, "snowball.json"));
            string outputFilename = Path.Combine(outputDirectory,
                $"{this.PackageInfo.PackageType}!{this.PackageInfo.Name}-{this.PackageInfo.Version}.snowball"
                    .ToLowerInvariant());
            if (File.Exists(outputFilename)) File.Delete(outputFilename);
          
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            Package.CopyFilesRecursively(new DirectoryInfo(Path.Combine(Path.Combine(packageRoot, "snowball"))),
                new DirectoryInfo(Path.Combine(Path.Combine(tempDir, "snowball"))));
            ZipFile.CreateFromDirectory(tempDir, outputFilename, CompressionLevel.NoCompression, false);
            Directory.Delete(tempDir, true);
            return Path.Combine(outputFilename);
            


        }
        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories().Where(dir => !dir.Name.StartsWith(".")))
            {
                Package.CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            }
            foreach (FileInfo file in source.GetFiles().Where(file => !file.Name.StartsWith(".")))
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name));
            }
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
