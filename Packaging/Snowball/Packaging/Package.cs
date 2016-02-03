using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Snowball.Packaging.Packagers;

namespace Snowball.Packaging
{
    public class Package
    {
        public PackageInfo PackageInfo { get; }
        public string PackageRoot { get; }
        public bool IsPacked { get; }
        public Package(PackageInfo packageInfo, bool isPacked, string packageRoot)
        {
            this.PackageInfo = packageInfo;
            this.IsPacked = isPacked;
            this.PackageRoot = packageRoot;
        }


        public static Package FromZip(string zipFile)
        {
            using (ZipArchive snowball = new ZipArchive(File.Open(zipFile, FileMode.Open), ZipArchiveMode.Read))
            {
                return
                    new Package(JsonConvert.DeserializeObject<PackageInfo>(
                            new StreamReader(snowball.GetEntry("snowball.json").Open()).ReadToEnd()), true, Path.GetFullPath(zipFile));
            }
        }

        public static Package LoadDirectory(string packageRoot)
        {
            if (!Directory.Exists(Path.Combine(packageRoot, "snowball")))
                throw new FileNotFoundException("Unable to locate package root");
            if (!File.Exists(Path.Combine(packageRoot, "snowball.json")))
                throw new FileNotFoundException("Unable to locate package manifest");
            var packageInfo =
                JsonConvert.DeserializeObject<PackageInfo>(File.ReadAllText(Path.Combine(packageRoot, "snowball.json")));
            return new Package(packageInfo, false, packageRoot);
        }

        public string Pack(string outputDirectory, bool overwrite = true)
        {
            string outputFilename = Path.Combine(outputDirectory,
               $"{this.PackageInfo.PackageType}!{this.PackageInfo.Name}-{this.PackageInfo.Version}.snowball"
                   .ToLowerInvariant());
            if (this.IsPacked)
            {
                File.Copy(this.PackageRoot, outputFilename, true);
            }
            else
            {
                if (!Directory.Exists(Path.Combine(this.PackageRoot, "snowball")))
                    throw new FileNotFoundException("Unable to locate package root");
                if (!File.Exists(Path.Combine(this.PackageRoot, "snowball.json")))
                    throw new FileNotFoundException("Unable to locate package manifest");

                if (File.Exists(outputFilename)) File.Delete(outputFilename); //todo implement no overwrite
                ZipFile.CreateFromDirectory(this.PackageRoot, outputFilename, CompressionLevel.NoCompression, false);
            }
            return outputFilename;
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories().Where(dir => !dir.Name.StartsWith(".")))
                Package.CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles().Where(file => !file.Name.StartsWith(".")))
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }
}
