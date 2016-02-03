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
using Snowball.Secure;
using FileMode = System.IO.FileMode;

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

        public NugetWrapper(string packagePath, PackageKeyStore packageKeyStore) : this(Package.FromZip(packagePath), packageKeyStore)
        {

        }
        public dynamic MakeNugetPackage()
        {
            string temporaryDirectory = NugetWrapper.GetTemporaryDirectory();
            string packagePath = this.Package.Pack(temporaryDirectory);
            byte[] packageSig = this.SignSnowball(packagePath);
            string packageSigPath = Path.Combine(temporaryDirectory, "signature.bin");
            File.WriteAllBytes(packageSigPath, packageSig);
            return new { PackagePath =  this.BuildNugetPackage(packagePath, packageSigPath), ReleaseInfo = this.MakeReleaseInfo() };
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
            }.Select(f => new ManifestFile { Source = f, Target = f.Replace(Path.GetDirectoryName(f), "") })
                .ToList();
            builder.PopulateFiles("", files);
            builder.Populate(metadata);
            string fileName = Path.GetTempFileName() + ".nupkg";
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
        private byte[] SignSnowball(string packagePath)
        {
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                rsaCrypt.FromXmlString(this.KeyPair.FullKey);
                string sha256Hash = NugetWrapper.HashSHA256(packagePath);
                return rsaCrypt.SignData(Encoding.UTF8.GetBytes(sha256Hash),
                    CryptoConfig.MapNameToOID("SHA512"));
            }
        }
        private static string HashSHA256(string fileName)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                using (var sha256 = SHA256.Create())
                {
                    return
                        BitConverter.ToString(sha256.ComputeHash(fileStream))
                            .Replace("-", string.Empty)
                            .ToLowerInvariant();
                }
            }
        }
        private static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
    }
}
