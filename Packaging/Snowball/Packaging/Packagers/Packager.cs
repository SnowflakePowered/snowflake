using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using Newtonsoft.Json;

namespace Snowball.Packaging.Packagers
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
        /// <param name="inputFile">The source input file. Expects a fully qualified path name</param>
        /// <param name="infoFile">The path to the snowball.json</param>
        /// <returns>The temporary directory with the unpacked Snowball</returns>
        public abstract string Make(string inputFile, string infoFile);

        /// <summary>
        /// Make a temporary directory with an unpacked snowball structure to be packed
        /// </summary>
        /// <param name="inputFile">The source input file  Expects a fully qualified path name</param>
        /// <param name="packageInfo">A packageInfo file</param>
        /// <returns>The temporary directory with the unpacked Snowball</returns>
        public abstract string Make(string inputFile, PackageInfo packageInfo);

        /// <summary>
        /// Sets up a temporary snowball directory given the resource root of the files
        /// </summary>
        /// <param name="resourceRoot">
        /// The fully qualified path of the resource root containing all resources of the item, excluding the inputFile.
        /// For plugins, this is the plugin resource folder named the same as the plugins
        /// For emulators, this is the emulator folder named the same as the emulatordef file
        /// For themes, this is the theme folder itself.
        /// </param>
        /// <param name="packageInfo">The packageInfo associated with this snowball</param>
        /// <returns>The path to the temporary directory</returns>
        protected static string CopyResourceFiles(string resourceRoot, PackageInfo packageInfo)
        {
            string resourceRootName = Path.GetFileName(resourceRoot);

            string tempDir = Packager.GetTemporaryDirectory();
            string snowballDir = Path.Combine(tempDir, "snowball");
            string resourceDir = Path.Combine(snowballDir, resourceRootName);

            Directory.CreateDirectory(snowballDir);
            Directory.CreateDirectory(resourceDir);
            if (Directory.Exists(resourceRoot))
            {
                Packager.CopyFilesRecursively(new DirectoryInfo(resourceRoot), new DirectoryInfo(resourceDir));
            }
            
            File.WriteAllText(Path.Combine(tempDir, "snowball.json"), JsonConvert.SerializeObject(packageInfo));
            return snowballDir;
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories().Where(dir => !dir.Name.StartsWith(".")))
                Package.CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles().Where(file => !file.Name.StartsWith(".")))
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }

        private static string GetTemporaryDirectory()
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
