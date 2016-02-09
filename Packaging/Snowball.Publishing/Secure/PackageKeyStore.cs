using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Snowball.Installation;

namespace Snowball.Publishing.Secure
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

    }
}
