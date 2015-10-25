using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
namespace Snowflake.Packaging.Publishing
{
    public class Signing
    {
       
        public static void SignSnowball(string plainFilePath)
        {
            using (FileStream snowballFile = File.Open(plainFilePath, FileMode.Open, FileAccess.Read))
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                string sha256Hash = Signing.HashSHA256(snowballFile);
                byte[] signature = rsaCrypt.SignData(Encoding.UTF8.GetBytes(sha256Hash), CryptoConfig.MapNameToOID("SHA512"));
                rsaCrypt.PersistKeyInCsp = false;
                
                string rsaPublic = rsaCrypt.ToXmlString(false);
                string outputSig = plainFilePath + ".sig";
                string outputKey = plainFilePath + ".key";
                if (File.Exists(outputSig)) File.Delete(outputSig);
                if (File.Exists(outputKey)) File.Delete(outputKey);
                File.WriteAllBytes(outputSig, signature);
                File.WriteAllText(outputKey, rsaPublic);
                
            }
        }
        public static bool VerifySnowball(string plainFilePath, string signaturePath, string rsaPublicPath)
        {
            string rsaPublic = File.ReadAllText(rsaPublicPath);
            byte[] signature = File.ReadAllBytes(signaturePath);
            using (FileStream snowballFile = File.Open(plainFilePath, FileMode.Open, FileAccess.Read))
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                string sha256Hash = Signing.HashSHA256(snowballFile);
                rsaCrypt.PersistKeyInCsp = false;
                rsaCrypt.FromXmlString(rsaPublic);
                return rsaCrypt.VerifyData(Encoding.UTF8.GetBytes(sha256Hash), CryptoConfig.MapNameToOID("SHA512"), signature);
            }
        }
        public static bool VerifySnowball(Stream snowballFile, byte[] signature, string rsaPublic)
        {
            using (RSACryptoServiceProvider rsaCrypt = new RSACryptoServiceProvider())
            {
                string sha256Hash = Signing.HashSHA256(snowballFile);
                rsaCrypt.PersistKeyInCsp = false;
                rsaCrypt.FromXmlString(rsaPublic);
                return rsaCrypt.VerifyData(Encoding.UTF8.GetBytes(sha256Hash), CryptoConfig.MapNameToOID("SHA512"), signature);
            }
        }
        public static string HashSHA256(Stream fileStream)
        {
            using (var sha256 = SHA256.Create())
                return BitConverter.ToString(sha256.ComputeHash(fileStream)).Replace("-", string.Empty).ToLowerInvariant();
        }
    }

}
