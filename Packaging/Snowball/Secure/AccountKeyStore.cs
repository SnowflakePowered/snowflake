using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
namespace Snowball.Secure
{
    internal class AccountKeyStore
    {
        private static readonly byte[] ProtectDataEntropy = Encoding.UTF8.GetBytes("SNOWFLAKESNOWBALLPWD");
        private readonly string keystoreFile;

        public AccountKeyStore() : this(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tokens.secure"))
        {
        }

        public AccountKeyStore(string keystoreFile)
        {
            this.keystoreFile = keystoreFile;
        }

        /// <summary>
        /// Sets tokens into the keystore
        /// </summary>
        /// <param name="nugetAuthToken">The nuget authorization token</param>
        /// <param name="githubAuthToken">The github authorization token</param>
        public void SetKeys(string nugetAuthToken, string githubAuthToken)
        {
            byte[] plaintextKeystore = Encoding.UTF8.GetBytes(nugetAuthToken + ";" + githubAuthToken);
            byte[] cipherKeystore = ProtectedData.Protect(plaintextKeystore, AccountKeyStore.ProtectDataEntropy,
                DataProtectionScope.CurrentUser);
            if (File.Exists(this.keystoreFile)) File.Delete(this.keystoreFile);
            File.WriteAllBytes(this.keystoreFile, cipherKeystore);

            //Attempt to destroy plaintext keystore in memory ASAP
            plaintextKeystore = null;
            cipherKeystore = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Gets the tokens stored in the keystore. 
        /// Index 0: NuGet API token
        /// Index 1: GitHub authorization token
        /// </summary>
        /// <param name="tokens">The tokens in the store.</param>
        public void GetTokens(out string[] tokens)
        {
            if (!File.Exists(this.keystoreFile)) tokens = new string[2];
            byte[] cipherKeystore = File.ReadAllBytes(this.keystoreFile);
            tokens = Encoding.UTF8.GetString(ProtectedData.Unprotect(cipherKeystore, AccountKeyStore.ProtectDataEntropy,
               DataProtectionScope.CurrentUser)).Split(';');
        }

    }
}
