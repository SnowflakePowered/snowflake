using System;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace Snowflake.Packaging.Publishing
{

    #region encryption

    /// <summary>
    /// http://stackoverflow.com/a/8874634
    /// </summary>
    internal class SecureIt
    {
        private static readonly byte[] entropy = Encoding.Unicode.GetBytes("SNOWFLAKESNOWBALLPWD");

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(
                Encoding.Unicode.GetBytes(SecureIt.ToInsecureString(input)),
                SecureIt.entropy,
                DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    SecureIt.entropy,
                    DataProtectionScope.CurrentUser);
                return SecureIt.ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
                secure.AppendChar(c);
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }

    #endregion

    public class Account
    {
        public static void SaveDetails(string githubToken, string nugetToken)
        {
            var cipherGithubToken = SecureIt.EncryptString(SecureIt.ToSecureString(githubToken));
            var cipherNugetToken = SecureIt.EncryptString(SecureIt.ToSecureString(nugetToken));
            Properties.Settings.Default.githubToken = cipherGithubToken;
            Properties.Settings.Default.nugetToken = cipherNugetToken;
            Properties.Settings.Default.Save();
            Console.WriteLine("Successfully stored and secured authorization details for NuGet and GitHub.");
        }


        public static async Task MakeRepoFork(string githubToken)
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball")) {Credentials = new Credentials(githubToken)};
            await gh.Repository.Forks.Create("SnowflakePowered-Packages", "snowball-packages", new NewRepositoryFork());
            Console.WriteLine("snowball-package has been forked to your GitHub account.");
        }

        public static async Task<string> CreateGithubToken(string username, string password,
            string twoFactorAuthenticationCode = "")
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            gh.Credentials = new Credentials(username, password);
            var authorization = new NewAuthorization("Snowball Packaging Publish", new[]
            {
                "public_repo",
                "repo:status",
                "repo",
                "user",
                "repo_deployment"
            }, Guid.NewGuid().ToString());
            ApplicationAuthorization appAuth;
            if (!string.IsNullOrWhiteSpace(twoFactorAuthenticationCode))
            {
                appAuth =
                    await
                        gh.Authorization.Create("e58f96af40993609ba34", "a7201762659569213809889c4873af6aa46a7c01",
                            authorization, twoFactorAuthenticationCode);
            }
            else
            {
                appAuth =
                    await
                        gh.Authorization.Create("e58f96af40993609ba34", "a7201762659569213809889c4873af6aa46a7c01",
                            authorization);
            }
            Console.WriteLine("A new authorization has been created for Snowflake on your GitHub account");
            return appAuth.Token;
        }

        public static string GetGithubToken()
        {
            //probably a bad idea but we'll do it anyways, we don't have much of a choice but to have the entire string in memory
            return SecureIt.ToInsecureString(SecureIt.DecryptString(Properties.Settings.Default.githubToken));
        }

        public static string GetNugetToken()
        {
            //same here, we don't have much of a choice but to have the entire string in memory
            return SecureIt.ToInsecureString(SecureIt.DecryptString(Properties.Settings.Default.nugetToken));
        }
    }
}