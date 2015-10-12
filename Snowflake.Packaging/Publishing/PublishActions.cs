using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net;
using Newtonsoft.Json;
using NuGet;
using Snowflake.Packaging.Snowball;
using Octokit;
namespace Snowflake.Packaging.Publishing
{
    internal static class PublishActions
    {
        public static string PackNuget(string snowballFilename)
        {
            Signing.SignSnowball(snowballFilename);
            string sigFile = snowballFilename + ".sig";
            string keyFile = snowballFilename + ".key";
            string packagePath = snowballFilename + ".nupkg";
            PackageInfo packageInfo = Package.FromZip(snowballFilename).PackageInfo;
            ManifestMetadata metadata = new ManifestMetadata()
            {
                Authors = String.Join(", ", packageInfo.Authors),
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
            }.Select(f => new ManifestFile { Source = f, Target = f.Replace(Path.GetDirectoryName(f), "") })
            .ToList(); 
            builder.PopulateFiles("", files);
            builder.Populate(metadata);
            using (FileStream stream = File.Open(packagePath, System.IO.FileMode.OpenOrCreate))
            {
                builder.Save(stream);
            }

            File.Delete(sigFile);
            File.Delete(keyFile);
            return packagePath;


        }
        public static void UploadNuget(string nupkg)
        {
            IPackage package = new OptimizedZipPackage(Path.GetFullPath(nupkg));
            var nugetServer = new PackageServer("https://www.nuget.org/", "userAgent");
            string token = Account.GetNugetToken();
            try
            {
                Console.WriteLine("Uploading snowball to NuGet");
                nugetServer.PushPackage(token, package, File.Open(nupkg, System.IO.FileMode.Open, FileAccess.Read).Length, 10000, false);
            }
            catch (WebException e)
            {
                if(e.Status == WebExceptionStatus.ProtocolError && ((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Conflict)
                {
                    Console.WriteLine("Error: Conflict occured - likely you are trying to reupload a versiont that has already been published.");
                }
                Console.WriteLine("Error: " + e.Message);
            }
            Console.WriteLine($"Uploaded package {nupkg} to NuGet");
            File.Delete(nupkg);

        }
        public async static Task MakeGithubIndex(PackageInfo packageInfo)
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            gh.Credentials = new Credentials(Account.GetGithubToken());
            var user = await gh.User.Current();
            string owner = user.Login;
            var forkRepository = await gh.Repository.Get(owner, "snowball-packages");
            var contents = await gh.Repository.Content.GetAllContents(owner, "snowball-packages", "index/");
            var masterContents = await gh.Repository.Content.GetAllContents("SnowflakePowered-Packages", "snowball-packages", "index/");

            bool update = masterContents.Select(content => content.Name).Contains(packageInfo.Name + ".json");
            var refs = gh.GitDatabase.Reference;
            var _refs = await refs.GetAll(owner, "snowball-packages");
            string masterSha = _refs.Where(branch => branch.Ref == "refs/heads/master").First().Object.Sha;
            string branchName = $"{packageInfo.Name}v{packageInfo.Version}-{Guid.NewGuid().ToString()}";
            var newBranch = new NewReference($"refs/heads/{branchName}", masterSha);
            await refs.Create(owner, "snowball-packages", newBranch);
                

            if(update)
            {
                var ghReleaseInfoContent = masterContents.Where(content => content.Name == packageInfo.Name + ".json").First();
                var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(ghReleaseInfoContent.Content);
                releaseInfo.ReleaseVersions.Add(packageInfo.Version);
                string newRelease = JsonConvert.SerializeObject(releaseInfo);
                var req = new UpdateFileRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}", newRelease, ghReleaseInfoContent.Sha);
                req.Branch = $"refs/heads/{branchName}";
                await gh.Repository.Content.UpdateFile(owner, "snowball-packages", $"index/{packageInfo.Name}.json", req);
            }
            else
            {
                var releaseInfo = new ReleaseInfo(packageInfo.Name, packageInfo.Description, packageInfo.Authors, new List<string>() { packageInfo.Version.ToString() }, packageInfo.Dependencies.Select(dep => dep.ToString()).ToList(), packageInfo.PackageType);
                string newRelease = JsonConvert.SerializeObject(releaseInfo);
                var req = new CreateFileRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}", newRelease);
                req.Branch = $"refs/heads/{branchName}";
                await gh.Repository.Content.CreateFile(owner, "snowball-packages", $"index/{packageInfo.Name}.json", req);
                
            }
            var pr = new NewPullRequest($"Add {packageInfo.PackageType} {packageInfo.Name} v{packageInfo.Version}", $"{user.Login}:{branchName}", "master");
            var prs = await gh.PullRequest.Create("SnowflakePowered-Packages", "snowball-packages", pr);

        }
    }
}
