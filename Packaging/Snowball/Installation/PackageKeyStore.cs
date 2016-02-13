using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Snowball.Packaging;

namespace Snowball.Installation
{
    public class PackageKeyStore
    {
        private string AppDataDirectory { get; }
        public PackageKeyStore(string appDataDirectory)
        {
            this.AppDataDirectory = Path.Combine(appDataDirectory, ".snowballkeys");
            if (!Directory.Exists(this.AppDataDirectory)) Directory.CreateDirectory(this.AppDataDirectory);

        }

        private string GenerateKeyPair()
        {
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                return rsaCrypt.ToXmlString(true);
            }
        }

        public PackageKeyPair GetKeyPair(string packageId)
        {
            string keyPairPath = Path.Combine(this.AppDataDirectory, $"{packageId}.keypair");
            if (File.Exists(keyPairPath))
            {
                using (var rsaCrypt = new RSACryptoServiceProvider())
                {
                    rsaCrypt.FromXmlString(File.ReadAllText(keyPairPath));
                    return new PackageKeyPair(rsaCrypt.ToXmlString(false), rsaCrypt.ToXmlString(true), packageId);
                }
            }
            else
            {
                File.WriteAllText(keyPairPath, this.GenerateKeyPair());
                return this.GetKeyPair(packageId); //yay for recursion;
            }
        }

        public static byte[] SignSnowball(Stream packageContents, PackageKeyPair keyPair)
        {
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                rsaCrypt.FromXmlString(keyPair.FullKey);
                byte[] sha256Hash = PackageKeyStore.HashSHA256(packageContents);
                return rsaCrypt.SignData(sha256Hash, CryptoConfig.MapNameToOID("SHA512"));
            }
        }
        public static bool VerifySnowball(Stream packageContents, PackageKeyPair keyPair, byte[] signature)
        {
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                rsaCrypt.FromXmlString(keyPair.PublicKey);
                byte[] sha256Hash = PackageKeyStore.HashSHA256(packageContents);
                return rsaCrypt.VerifyData(sha256Hash, CryptoConfig.MapNameToOID("SHA512"), signature);
            }
        }
        private static byte[] HashSHA256(Stream contents)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(contents);
            }
        }
    }
}
