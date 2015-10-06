using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Snowflake.Packaging.Snowball
{
    public class Package
    {
        public PackageInfo PackageInfo { get; }
        public IList<string> PackageManifest { get; }
        public Package(PackageInfo packageInfo, IList<string> packageManifest)
        {
            this.PackageInfo = packageInfo;
            this.PackageManifest = packageManifest;
        }

        public static Package LoadDirectory(string packageRoot)
        {
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball"))) throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            var packageInfo = JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText(Path.Combine(packageRoot, "snowball.json")));
            IList<string> packageManifest =
                Directory.EnumerateFiles(Path.Combine(packageRoot, "snowball"), "*", SearchOption.AllDirectories)
                    .Select(fileName => fileName.Replace(Path.Combine(packageRoot, "snowball"), "~")).ToList();
            return new Package(packageInfo, packageManifest);
        }

        public string Pack(string outputDirectory, string packageRoot)
        {
            var tempDir = Package.GetTemporaryDirectory();
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball"))) throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            Directory.CreateDirectory(Path.Combine(tempDir, "snowball"));
            foreach (string fileName in this.PackageManifest.Select(fileName => fileName.Replace("~", Path.Combine(packageRoot, "snowball"))))
            {
                File.Copy(fileName, Path.Combine(tempDir, "snowball", Path.GetFileName(fileName)));
            }
            File.Copy(Path.Combine(packageRoot, "snowball.json"), Path.Combine(tempDir, "snowball.json"));
            string outputFilename = Path.Combine(outputDirectory,
                $"{this.PackageInfo.PackageType}!{this.PackageInfo.Name}@{this.PackageInfo.Version}.snowball"
                    .ToLowerInvariant());
            if (File.Exists(outputFilename)) File.Delete(outputFilename);
            ZipFile.CreateFromDirectory(tempDir, outputFilename, CompressionLevel.NoCompression, false);
            Directory.Delete(tempDir, true);
            return Path.Combine(outputFilename);
        }

        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

    }
}
