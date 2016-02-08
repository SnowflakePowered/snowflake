using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet;
using Octokit;
using Snowball.Installation;
using Snowball.Packaging;
using Snowball.Secure;

namespace Snowball.Publishing
{
    public class Publisher
    {
        NugetWrapper WrappedPackage { get; }
        Tuple<string, ReleaseInfo> WrappedPackageResult { get; }
        AccountKeyStore AccountKeyStore { get; }
        LocalRepository LocalRepository { get; }
        GithubAccountManager GithubAccountManager { get; }
        public Publisher(NugetWrapper wrappedPackage, AccountKeyStore accountKeystore, LocalRepository localRepository)
        {
            this.WrappedPackage = wrappedPackage;
            this.WrappedPackageResult = wrappedPackage.MakeNugetPackage();
            this.AccountKeyStore = accountKeystore;
            this.LocalRepository = localRepository;
            this.GithubAccountManager = new GithubAccountManager(accountKeystore);
        }

        public void PushPackageToNuGet(int timeout = 300)
        {
            IPackage package = new OptimizedZipPackage(this.WrappedPackageResult.Item1);
            var nugetServer = new PackageServer("https://www.nuget.org/", "userAgent");
            string[] tokens;
            this.AccountKeyStore.GetTokens(out tokens);
            try
            {
                nugetServer.PushPackage(tokens[0], package, package.GetStream().Length, timeout*1000, false);
            }
            catch (WebException e)
            {
                if (e.Status == WebExceptionStatus.ProtocolError &&
                    ((HttpWebResponse) e.Response).StatusCode == HttpStatusCode.Conflict)
                {
                    throw new InvalidOperationException("Conflict occured - likely you are trying to reupload a version that has already been published.");
                    //fail fast if there's a version conflict
                }
            }
            finally
            {
                tokens = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        
        public NewPullRequest MakePullRequest()
        {
            return Task.Run(async () =>
            {
                if ((await this.LocalRepository.UpdatedRequired()))
                {
                    await this.LocalRepository.UpdateRepository();
                }
                ReleaseInfo localReleaseInfo = this.WrappedPackageResult.Item2;
                PackageInfo packageInfo = this.WrappedPackage.Package.PackageInfo;
                bool indexIsUpdate = this.LocalRepository.CheckPublishUpdate(localReleaseInfo);
                ReleaseInfo publishReleaseInfo = this.LocalRepository.MergeReleaseVersions(localReleaseInfo);
                NewReference releaseBranch =
                    await this.GithubAccountManager.CreateRemotePersonalBranch(packageInfo);
                if (indexIsUpdate)
                {
                    await
                        this.GithubAccountManager.UpdateRemotePersonalRelease(packageInfo, publishReleaseInfo,
                            releaseBranch);
                }
                else
                {
                    await
                        this.GithubAccountManager.CreateRemotePersonalRelease(packageInfo, publishReleaseInfo,
                            releaseBranch);
                }
                return
                    new NewPullRequest(
                        $"{(indexIsUpdate ? "Update" : "Add")} {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                        $"{this.GithubAccountManager.CurrentUser.Login}:{GithubAccountManager.GetRelativeBranchName(releaseBranch.Ref)}",
                        "master")
                    {
                        Body = Publisher.GeneratePullRequestReport(localReleaseInfo, publishReleaseInfo, indexIsUpdate)
                    };
            }).Result;
        }

        public PullRequest PublishPullRequest(NewPullRequest pullRequest)
        {
           return Task.Run(async () => await this.GithubAccountManager.Client.PullRequest.Create("SnowflakePowered-Packages", "snowball-packages", pullRequest)).Result;
        }

        private static string GeneratePullRequestReport(ReleaseInfo localReleaseInfo, ReleaseInfo publishReleaseInfo,
            bool isUpdate)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"This Pull Request {(isUpdate ? "**updates**" : "**adds**")} the package _{localReleaseInfo.PackageType}_.{localReleaseInfo.Name}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"**Package Name**: {localReleaseInfo.Name}");
            stringBuilder.AppendLine($"**Package Type**: {localReleaseInfo.PackageType}");
            stringBuilder.AppendLine($"**Version**: {localReleaseInfo.ReleaseVersions.First().Key.ToNormalizedString()}");
            stringBuilder.AppendLine($"**Description**: _{localReleaseInfo.Description}_");
            stringBuilder.AppendLine($"**NuGet Package URL**: { LocalRepository.GetNugetPage(publishReleaseInfo)}");
            stringBuilder.AppendLine($"**NuGet Download URL**: { LocalRepository.GetNugetDownload(publishReleaseInfo)}");

            if (!publishReleaseInfo.PublicKey.Equals(localReleaseInfo.PublicKey))
            {
                stringBuilder.AppendLine($"**!!PUBLIC KEYS ARE MISMATCHED FROM PREVIOUS VERSION!!**");
            }

            stringBuilder.AppendLine(
                "_Your application is currently under review. We will merge this pull request as soon as we can verify the package_");

            return stringBuilder.ToString();
        }
    }
}
