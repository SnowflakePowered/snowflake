using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NuGet;
using Octokit;
using Snowball.Packaging;
using Snowball.Installation;
using FileMode = System.IO.FileMode;
using Snowball.Publishing.Secure;

namespace Snowball.Publishing
{
    public class NugetWrapper
    {
        public Package Package { get; }
        public PackageKeyPair KeyPair { get; }

        public NugetWrapper(Package package, PackageKeyStore packageKeyStore)
        {
            this.Package = package;
            this.KeyPair = packageKeyStore.GetKeyPair(this.Package.PackageInfo.Name);
        }

        public NugetWrapper(string packagePath, PackageKeyStore packageKeyStore) : this(Package.LoadZip(packagePath), packageKeyStore)
        {

        }

        public Tuple<string, ReleaseInfo> MakeNugetPackage()
        {
            string temporaryDirectory = NugetWrapper.GetTemporaryDirectory();
            string packagePath = this.Package.Pack(temporaryDirectory);
            byte[] packageSig = PackageKeyStore.SignSnowball(File.OpenRead(packagePath), this.KeyPair);
            string packageSigPath = Path.Combine(temporaryDirectory, "signature.bin");
            File.WriteAllBytes(packageSigPath, packageSig);
            return new Tuple<string, ReleaseInfo>(this.BuildNugetPackage(packagePath, packageSigPath), this.MakeReleaseInfo());
        }
        
        private string BuildNugetPackage(string packagePath, string packageSigPath)
        {
            PackageInfo packageInfo = this.Package.PackageInfo;
            ManifestMetadata metadata = new ManifestMetadata
            {
                Authors = String.Join(", ", packageInfo.Authors),
                Version = packageInfo.Version.ToString(),
                Id = $"snowflake-snowball-{packageInfo.PackageType}-{packageInfo.Name}",
                Description = packageInfo.Description,
                Tags = "snowflake snowball"
            };


            PackageBuilder builder = new PackageBuilder();
            var files = new string[2]
            {
                packagePath,
                packageSigPath
            }
            .Select(f => new ManifestFile { Source = f, Target = f.Replace(Path.GetDirectoryName(f), "") })
                .ToList();
            builder.PopulateFiles("", files);
            builder.Populate(metadata);
            string fileName = packagePath + ".nupkg";
            using (FileStream stream = File.Create(fileName, 1024, FileOptions.Asynchronous))
            {
                builder.Save(stream);
                long streamLength = stream.Length;
                stream.Close();
                return fileName;
            }

        }
        private ReleaseInfo MakeReleaseInfo()
        {
            return new ReleaseInfo(this.Package.PackageInfo, this.KeyPair);
        }
        
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
