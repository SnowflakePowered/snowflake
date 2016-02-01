using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Snowball.Packaging
{
    /// <summary>
    /// Represents a handler for a certain package type
    /// </summary>
    public abstract class Packager
    {
        /// <summary>
        /// The domain of the package type (folder within the snowflake root that these 
        /// </summary>
        public string FolderDomain { get; }
        /// <summary>
        /// The type of the package
        /// </summary>
        public PackageType PackageType { get; }
 
        protected Packager(string folderDomain, PackageType packageType)
        {
            this.FolderDomain = folderDomain;
            this.PackageType = packageType;
        }

        /// <summary>
        /// Extracts a package to the folder domain
        /// </summary>
        /// <param name="packageZipArchive">The package zip archive (.snowball) file</param>
        /// <returns></returns>
        public IList<string> ExtractPackage(ZipArchive packageZipArchive)
        {
            IList<string> uninstallManifest = new List<string>();
            if (packageZipArchive.Entries.FirstOrDefault(entry => entry.FullName != "snowball.json") == null)
                return uninstallManifest;
            foreach (ZipArchiveEntry entry in packageZipArchive.Entries.Where(entry => entry.FullName != "snowball.json"))
            {
                string extractPath = Path.Combine(this.FolderDomain, entry?.FullName.Remove(0, 9));
                //9 chars in snowball/
                if (entry.FullName.EndsWith(@"\"))
                {
                    Directory.CreateDirectory(entry.FullName);
                    continue;
                }
                if (!Directory.Exists(Path.GetDirectoryName(extractPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                entry.ExtractToFile(extractPath, true);
                uninstallManifest.Add(extractPath);
            }
            return uninstallManifest;
        }

        /// <summary>
        /// Make a temporary directory with an unpacked snowball structure to be packed 
        /// </summary>
        /// <param name="inputFile">The source input file</param>
        /// <param name="infoFile">The path to the snowball.json</param>
        /// <returns>The temporary directory with the unpacked Snowball</returns>
        public abstract string Make(string inputFile, string infoFile);

        /// <summary>
        /// Make a temporary directory with an unpacked snowball structure to be packed
        /// </summary>
        /// <param name="inputFile">The source input file</param>
        /// <param name="infoFile">A packageInfo file</param>
        /// <returns>The temporary directory with the unpacked Snowball</returns>
        public abstract string Make(string inputFile, PackageInfo infoFile);
 
        protected static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

    }


    public enum PackageType
    {
        Plugin,
        Theme,
        EmulatorAssembly
    }
}
