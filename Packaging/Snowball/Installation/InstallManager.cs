using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;

namespace Snowball.Installation
{
    public class InstallManager
    {
        public string AppDataPath { get; }
        public LocalRepository PackageRepository { get; }
        public PackageKeyStore KeyStore { get; }

        public InstallManager(string appDataPath, LocalRepository localRepository)
        {
            if (!Directory.Exists(Path.Combine(appDataPath, ".snowballmanifest")))
                Directory.CreateDirectory(Path.Combine(appDataPath, ".snowballmanifest"));
            this.AppDataPath = Path.GetFullPath(appDataPath);
            this.PackageRepository = localRepository;
        }

        public void InstallNupkg(ReleaseInfo releaseInfo, ZipArchive nupkgPackage)
        {
            ZipArchiveEntry snowballEntry = nupkgPackage.Entries.FirstOrDefault(entry => entry.FullName.EndsWith(".snowball"));
            if (snowballEntry == null) throw new FileNotFoundException("Unable to find snowball package in nupkg");
            Stream snowballContents = snowballEntry.Open();
            ZipArchive snowballPackage = new ZipArchive(snowballContents);
            Stream signatureStream = snowballPackage.GetEntry("signature.bin").Open();
            byte[] signature = new byte[signatureStream.Length];
            signatureStream.Read(signature, 0, (int)signatureStream.Length);
            if (!PackageKeyStore.VerifySnowball(snowballContents, this.KeyStore.GetKeyPair(releaseInfo.Name), signature))
                throw new InvalidOperationException("Key mismatch");
            this.InstallRawPackage(snowballPackage);
        }

        public void InstallRawPackage(ZipArchive snowballPackage)
        {
            PackageInfo packageObj =
                JsonConvert.DeserializeObject<PackageInfo>(
                    new StreamReader(snowballPackage.GetEntry("snowball.json").Open()).ReadToEnd());
           
            IList<string> uninstallManifest = new List<string>();
            string packageInstallPath = null;
            switch (packageObj.PackageType)
            {
                case PackageType.Plugin:
                    packageInstallPath = Path.Combine(this.AppDataPath, "plugins");
                    break;
                case PackageType.EmulatorAssembly:
                    packageInstallPath = Path.Combine(this.AppDataPath, "emulators");
                    break;
                case PackageType.Theme:
                    packageInstallPath = Path.Combine(this.AppDataPath, "themes");
                    break;
            }
            if (packageInstallPath == null) throw new InvalidOperationException("Did not find package type");
            foreach (ZipArchiveEntry entry in snowballPackage.Entries.Where(entry => entry.FullName != "snowball.json"))
            {
                string extractPath = Path.Combine(packageInstallPath, entry.FullName.Remove(0, 9));
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
            File.WriteAllLines(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageObj.Name}.manifest"),
                uninstallManifest);
            Console.WriteLine($"Installed {packageObj.PackageType} {packageObj.Name} v{packageObj.Version}");
        }

        public void UninstallPackage(string packageId)
        {
            IList<string> uninstallManifest =
                File.ReadAllLines(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageId}.manifest"));
            foreach (string file in uninstallManifest)
            {
                string directoryName = Path.GetDirectoryName(file);
                File.Delete(file);
                if (!Directory.EnumerateFiles(directoryName).Any() &&
                    Path.GetFullPath(Path.GetDirectoryName(directoryName)) != this.AppDataPath)
                    Directory.Delete(Path.GetDirectoryName(file));
            }
            File.Delete(Path.Combine(this.AppDataPath, ".snowballmanifest", $"{packageId}.manifest"));
        }
    }
}