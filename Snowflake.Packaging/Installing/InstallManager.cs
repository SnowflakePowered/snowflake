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
        public IDictionary<string, IList<string>> FileManifest { get; }
        public InstallManager(string appDataPath)
        {
            this.FileManifest = JsonConvert.DeserializeObject<IDictionary<string, IList<string>>>(File.ReadAllText(Path.Combine(appDataPath, ".snowballdb")));
        }


        public void InstallPackages (IList<string> packageQueue, string tempPath, string appDataDirectory)
        {

            foreach(string packageString in packageQueue)
            {

                string packageId = packageString.Split('@')[0];
                string packageVersion = packageString.Split('@')[1];
                string packagePath =  Directory.EnumerateFiles(tempPath).FirstOrDefault(path => path.Contains($"!{packageId}-"));
                if (!String.IsNullOrWhiteSpace(packagePath))
                {
                    var zip = new ZipArchive(File.Open(packagePath, FileMode.Open), ZipArchiveMode.Read, false);
                    var packageObj = JsonConvert.DeserializeObject<PackageInfo>(new StreamReader(zip.GetEntry("snowball.json").Open()).ReadToEnd());
                    if (!this.FileManifest.ContainsKey(packageId)) this.FileManifest.Add(packageId, new List<string>());
                    switch (packageObj.PackageType)
                    {
                        case PackageType.Plugin:
                            string pluginPath = Path.Combine(appDataDirectory, "plugins");
                            foreach(ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                            {
                                string extractPath = Path.Combine(pluginPath, entry.FullName.Remove(9)); //9 chars in snowball/
                                entry.ExtractToFile(extractPath);
                                this.FileManifest[packageId].Add(extractPath);
                            }
                            break;
                        case PackageType.EmulatorAssembly:
                            string emulatorPath = Path.Combine(appDataDirectory, "emulators");
                            foreach (ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                            {
                                string extractPath = Path.Combine(emulatorPath, entry.FullName.Remove(9)); //9 chars in snowball/
                                entry.ExtractToFile(extractPath);
                                this.FileManifest[packageId].Add(extractPath);
                            }
                            break;
                        case PackageType.Theme:
                            string themePath = Path.Combine(appDataDirectory, "themes");
                            foreach (ZipArchiveEntry entry in zip.Entries.Where(entry => entry.FullName != "snowball.json"))
                            {
                                string extractPath = Path.Combine(themePath, entry.FullName.Remove(9)); //9 chars in snowball/
                                entry.ExtractToFile(extractPath);
                                this.FileManifest[packageId].Add(extractPath);
                            }
                            break;
                    }
                };
            }
        }
    }
}
