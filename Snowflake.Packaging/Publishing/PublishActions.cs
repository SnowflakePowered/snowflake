using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet;
using Octokit;
using Snowflake.Packaging.Snowball;

namespace Snowflake.Packaging.Publishing
{
    internal static class PublishActions
    {
        public static string PackNuget(string snowballFilename)
        {
            Signing.SignSnowball(snowballFilename);
            string sigFile = snowballFilename + ".sig";
            string keyFile = snowballFilename + ".key";
            string packagePath = Path.GetTempFileName() + ".nupkg";
            PackageInfo packageInfo = Package.FromZip(snowballFilename).PackageInfo;
            ManifestMetadata metadata = new ManifestMetadata
            {
                Authors = string.Join(", ", packageInfo.Authors),
                Version = packageInfo.Version.ToString(),
                Id = $"snowflake-snowball-{packageInfo.PackageType}-{packageInfo.Name}",
                Description = packageInfo.Description,
                Tags = "snowflake snowball"
            };

            Console.WriteLine($"Packing {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}");
            PackageBuilder builder = new PackageBuilder();
            var files = new string[3]
            {
                Path.GetFullPath(snowballFilename),
                Path.GetFullPath(keyFile),
                Path.GetFullPath(sigFile)
            }.Select(f => new ManifestFile {Source = f, Target = f.Replace(Path.GetDirectoryName(f), "")})
                .ToList();
            builder.PopulateFiles("", files);
            builder.Populate(metadata);
            string fileName = Path.GetTempFileName();
            using (FileStream stream = File.Create(fileName, 1024, FileOptions.Asynchronous))
            {
                builder.Save(stream);
                long streamLength = stream.Length;
                stream.Close();
                IPackage package = new OptimizedZipPackage(fileName);
                var nugetServer = new PackageServer("https://www.nuget.org/", "userAgent");
                File.Delete(sigFile);
                File.Delete(keyFile);
                string token = Account.GetNugetToken();
                try
                {
                    Console.WriteLine("Uploading snowball to NuGet");
                    nugetServer.PushPackage(token, package, streamLength, 1800000, false);
                    Console.WriteLine($"Successfully uploaded package {package.Id} v{package.Version} to NuGet");
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError &&
                        ((HttpWebResponse) e.Response).StatusCode == HttpStatusCode.Conflict)
                    {
                        Console.WriteLine(
                            "Error: Conflict occured - likely you are trying to reupload a version that has already been published.");
                    }
                    Console.WriteLine("Error: " + e.Message);
                }
                stream.Close();
            }

            return packagePath;
        }

        public static async Task MakeGithubIndex(PackageInfo packageInfo)
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball"))
            {
                Credentials = new Credentials(Account.GetGithubToken())
            };
            var user = await gh.User.Current();
            string owner = user.Login;
            string pluginIndexFile = packageInfo.Name + ".json";
            var forkRepository = await gh.Repository.Get(owner, "snowball-packages");
            var contents = await gh.Repository.Content.GetAllContents(owner, "snowball-packages", "index/");
            var masterContents =
                await gh.Repository.Content.GetAllContents("SnowflakePowered-Packages", "snowball-packages", "index/");

            bool update = masterContents.Select(content => content.Name).Contains(pluginIndexFile);
            Console.WriteLine(
                $"Creating index entry for {packageInfo.PackageType}-{packageInfo.Name} on personal branch");
            var refs = gh.GitDatabase.Reference;
            var _refs = await refs.GetAll("SnowflakePowered-Packages", "snowball-packages");
            string masterSha = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
            string branchName = $"{packageInfo.Name}v{packageInfo.Version}-{Guid.NewGuid()}";
            var newBranch = new NewReference($"refs/heads/{branchName}", masterSha);
            await refs.Create(owner, "snowball-packages", newBranch);


            if (update)
            {
                var ghReleaseInfoContent = masterContents.First(content => content.Name == pluginIndexFile);
                string jsonFile = await new WebClient().DownloadStringTaskAsync(ghReleaseInfoContent.DownloadUrl);
                var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(jsonFile);
                releaseInfo.ReleaseVersions.Add(packageInfo.Version, packageInfo.Dependencies);
                string newRelease = JsonConvert.SerializeObject(releaseInfo, Formatting.Indented) + Environment.NewLine;
                var req =
                    new UpdateFileRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                        newRelease, ghReleaseInfoContent.Sha) {Branch = $"refs/heads/{branchName}"};
                await gh.Repository.Content.UpdateFile(owner, "snowball-packages", $"index/{pluginIndexFile}", req);
            }
            else
            {
                var releaseInfo = new ReleaseInfo(packageInfo);
                string newRelease = JsonConvert.SerializeObject(releaseInfo, Formatting.Indented) + Environment.NewLine;
                var req =
                    new CreateFileRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                        newRelease) {Branch = $"refs/heads/{branchName}"};
                await gh.Repository.Content.CreateFile(owner, "snowball-packages", $"index/{pluginIndexFile}", req);
            }
            Console.WriteLine($"Submitting {packageInfo.PackageType}-{packageInfo.Name} to master repository");
            var pr = new NewPullRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}",
                $"{user.Login}:{branchName}", "master");
            var prs = await gh.PullRequest.Create("SnowflakePowered-Packages", "snowball-packages", pr);
            Console.WriteLine(
                $"Submission complete. Please wait for approval for at {prs.HtmlUrl} before your package is available.");
        }
    }
}