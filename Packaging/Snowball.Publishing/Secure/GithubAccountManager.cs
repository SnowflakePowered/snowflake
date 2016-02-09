using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octokit;
using Snowball.Packaging;
using Snowball.Installation;

namespace Snowball.Publishing.Secure
{
    public class GithubAccountManager
    {
        AccountKeyStore AccountKeyStore { get; }
        internal GitHubClient Client { get; }
        public User CurrentUser { get; }
        internal GithubAccountManager(AccountKeyStore accountKeyStore)
        {
            this.AccountKeyStore = accountKeyStore;
            string[] tokens;
            this.AccountKeyStore.GetTokens(out tokens);
            if (tokens.Length < 2) throw new InvalidOperationException("GitHub account details missing. Reauthorization required.");
            this.Client = this.GetClient(tokens);
            tokens = null; //get tokens out of mem asap
            GC.Collect();
            GC.WaitForPendingFinalizers();
            this.CurrentUser = Task.Run(async () => await this.Client.User.Current()).Result;
        }

        private GitHubClient GetClient(string[] tokens)
        {
            this.AccountKeyStore.GetTokens(out tokens);
            var githubClient = new GitHubClient(new ProductHeaderValue("snowball"))
            {
                 Credentials = new Credentials(tokens[1]),
            };
            return githubClient;
        }

        public static async Task<string> InitializeGithubDetails(string username, string password,
            string twoFactorAuthenticationCode = "")
        {
            
            var githubClient = new GitHubClient(new ProductHeaderValue("snowball"))
            {
                Credentials = new Credentials(username, password)
            };
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
                       githubClient.Authorization.Create("e58f96af40993609ba34", "a7201762659569213809889c4873af6aa46a7c01", //find some way to obfuscate this
                            authorization, twoFactorAuthenticationCode);
            }
            else
            {
                appAuth =
                    await
                        githubClient.Authorization.Create("e58f96af40993609ba34", "a7201762659569213809889c4873af6aa46a7c01",
                            authorization);
            }
            return appAuth.Token;
        }

        internal async Task<NewReference> CreateRemotePersonalBranch(PackageInfo packageInfo)
        {
            var refs = this.Client.Git.Reference;
            var _refs = await refs.GetAll("SnowflakePowered-Packages", "snowball-packages");
            string masterSha = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
            string branchName = $"{packageInfo.Name}v{packageInfo.Version}-{Guid.NewGuid()}";
            var newBranch = new NewReference($"refs/heads/{branchName}", masterSha);
            await refs.Create(this.CurrentUser.Login, "snowball-packages", newBranch);
            return newBranch;
        }

        internal async Task<RepositoryContent> GetUpdateFileContent(PackageInfo packageInfo)
        {
            var masterContents =
                 await this.Client.Repository.Content.GetAllContents("SnowflakePowered-Packages", "snowball-packages", "index/");
            return masterContents.First(content => content.Name == $"{packageInfo.Name}.{packageInfo.PackageType}.rel.json".ToLowerInvariant());
        }

        internal async Task UpdateRemotePersonalRelease(PackageInfo packageInfo, ReleaseInfo releaseInfo, NewReference branch)
        {
            string newRelease = JsonConvert.SerializeObject(releaseInfo, Formatting.Indented) + Environment.NewLine;
            var releaseInfoContent = await this.GetUpdateFileContent(packageInfo);
            var req = new UpdateFileRequest($"Update {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                newRelease, releaseInfoContent.Sha)
            {
                Branch = branch.Ref
            };
            await this.Client.Repository.Content.UpdateFile(this.CurrentUser.Login, "snowball-packages", $"index/{$"{packageInfo.Name}.{packageInfo.PackageType}.rel.json".ToLowerInvariant()}", req);
        }

        internal async Task CreateRemotePersonalRelease(PackageInfo packageInfo, ReleaseInfo releaseInfo, NewReference branch)
        {
            string newRelease = JsonConvert.SerializeObject(releaseInfo, Formatting.Indented) + Environment.NewLine;
            var req = new CreateFileRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                newRelease)
            {
                Branch = branch.Ref
            };
            await this.Client.Repository.Content.CreateFile(this.CurrentUser.Login, "snowball-packages", $"index/{$"{packageInfo.Name}.{packageInfo.PackageType}.rel.json".ToLowerInvariant()}", req);
        }

        internal static string GetRelativeBranchName(string fullyQualifiedRef)
        {
            return fullyQualifiedRef.Replace("refs/heads/", "");
        }

    }
}
