using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using Snowflake.Packaging.Snowball;
namespace Snowflake.Packaging.Installing
{
    public class InstallManager
    {
        public string AppDataPath { get; }
        public InstallManager(string appDataPath)
        {
            if (!Directory.Exists(Path.Combine(appDataPath, ".snowballmanifest")))
                Directory.CreateDirectory(Path.Combine(appDataPath, ".snowballmanifest"));
            this.AppDataPath = Path.GetFullPath(appDataPath);
        }

        public void InstallSinglePackage(string packagePath)
        {
            if (String.IsNullOrWhiteSpace(packagePath)) return;
            var zip = new ZipArchive(File.Open(packagePath, FileMode.Open), ZipArchiveMode.Read, false);
            var packageObj =
                JsonConvert.DeserializeObject<PackageInfo>(
                    new StreamReader(zip.GetEntry("snowball.json").Open()).ReadToEnd());
            string packageId = packageObj.Name;
            IList<string> uninstallManifest = new List<string>();
            switch (packageObj.PackageType)
            {
                case PackageType.Plugin:
                    string pluginPath = Path.Combine(this.AppDataPath, "plugins");
                    foreach (ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                    {
                        string extractPath = Path.Combine(pluginPath, entry.FullName.Remove(0, 9));
                        //9 chars in snowball/
                        if (!Directory.Exists(Path.GetDirectoryName(extractPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                        entry.ExtractToFile(extractPath, true);
                        uninstallManifest.Add(extractPath);
                    }
                    break;
                case PackageType.EmulatorAssembly:
                    string emulatorPath = Path.Combine(this.AppDataPath, "emulators");
                    foreach (ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                    {
                        string extractPath = Path.Combine(emulatorPath, entry.FullName.Remove(0,9));
                        //9 chars in snowball/
                        if (!Directory.Exists(Path.GetDirectoryName(extractPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                        entry.ExtractToFile(extractPath, true);
                        uninstallManifest.Add(extractPath);
                    }
                    break;
                case PackageType.Theme:
                    string themePath = Path.Combine(this.AppDataPath, "themes");
                    foreach (ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                    {
                        string extractPath = Path.Combine(themePath, entry.FullName.Remove(0,9));
                        //9 chars in snowball/
                        if (!Directory.Exists(Path.GetDirectoryName(extractPath)))
                            Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                        entry.ExtractToFile(extractPath, true);
                        uninstallManifest.Add(extractPath);
                    }
                    break;
            }
            File.WriteAllLines(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageObj.Name}.manifest"), uninstallManifest);
        }

        public void UninstallPackage(string packageId)
        {
            IList<string> uninstallManifest = File.ReadAllLines(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageId}.manifest"));
            foreach (string file in uninstallManifest)
            {
                string directoryName = Path.GetDirectoryName(file);
                File.Delete(file);
                if (!Directory.EnumerateFiles(directoryName).Any() && Path.GetFullPath(Path.GetDirectoryName(directoryName)) != this.AppDataPath)
                {
                    Directory.Delete(Path.GetDirectoryName(file));
                }
            }

            File.Delete(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageId}.manifest"));

        }

        public void InstallPackages (IList<string> packageQueue, string tempPath, string appDataDirectory)
        {

            foreach(string packageString in packageQueue)
            {

                string packageId = packageString.Split('@')[0];
                string packageVersion = packageString.Split('@')[1];
                string packagePath =  Directory.EnumerateFiles(tempPath).FirstOrDefault(path => path.Contains($"!{packageId}-"));
                
                };
            }
        }
    }

