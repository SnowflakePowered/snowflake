using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net;
using Octokit;
using Newtonsoft.Json;
using NuGet;
using Snowflake.Packaging.Snowball;
using Snowflake.Packaging.Publishing;

namespace Snowflake.Packaging.Installing
{
    public class LocalRepository
    {
        public string AppDataDirectory { get; }
        public string RepositoryOrg{ get; }
        public string RepositoryName { get; }
        private string ArchivePath { get; }
        private ZipArchive RepositoryZip { get; set; }
        public LocalRepository(string appDataDirectory, string repositorySlug="SnowflakePowered-Packages/snowball-packages")
        {
            this.AppDataDirectory = appDataDirectory;
            this.RepositoryOrg = repositorySlug.Split('/')[0];
            this.RepositoryName = repositorySlug.Split('/')[1];
            this.ArchivePath = Path.Combine(this.AppDataDirectory, "snowball.repo");
            if (File.Exists(this.ArchivePath)) this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));

        }

        public async Task<bool> UpdatedRequired()
        {
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.cachedRepoHash) || this.RepositoryZip == null) return true;
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            var _refs = await gh.GitDatabase.Reference.GetAll(this.RepositoryOrg, this.RepositoryName);
            string remoteRepoHash = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
            return remoteRepoHash != Properties.Settings.Default.cachedRepoHash;
        }
        public async Task UpdateRepository()
        {
            using(var downloader = new WebClient())
            {
                this.RepositoryZip?.Dispose();
                if (File.Exists(this.ArchivePath)) File.Delete(this.ArchivePath);
                Console.WriteLine($"Updating repository cache from {this.RepositoryOrg}/{this.RepositoryName}");
                await downloader.DownloadFileTaskAsync($"https://github.com/{this.RepositoryOrg}/{this.RepositoryName}/archive/master.zip", this.ArchivePath);
                var gh = new GitHubClient(new ProductHeaderValue("snowball"));
                var _refs = await gh.GitDatabase.Reference.GetAll(this.RepositoryOrg, this.RepositoryName);
                string remoteRepoHash = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
                Properties.Settings.Default.cachedRepoHash = remoteRepoHash;
                Properties.Settings.Default.Save();
                this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));
            }
        }

        public bool PluginExistsInRepository(string packageId)
        {
            return this.RepositoryZip?.Entries.Where(entry => StringComparer.InvariantCultureIgnoreCase.Equals(entry.Name, $"{packageId}.json")).Any() ?? false;
        }

        public ReleaseInfo GetReleaseInfo(string packageId)
        {
            string jsonFile = new StreamReader(this.RepositoryZip?.Entries.FirstOrDefault(entry => StringComparer.InvariantCultureIgnoreCase.Equals(entry.Name, $"{packageId}.json"))?.Open()).ReadToEnd();
            return jsonFile != null ? JsonConvert.DeserializeObject<ReleaseInfo>(jsonFile) : null;
        }


        public IEnumerable<ReleaseInfo> ResolveDependencies(string packageId, SemanticVersion releaseVersion = null)
        {
            IList<ReleaseInfo> releaseInfos = new List<ReleaseInfo>();
            var initialReleaseInfo = this.GetReleaseInfo(packageId);
            if (initialReleaseInfo == null) return null;
            releaseInfos.Add(initialReleaseInfo);
            var versionDeps = releaseVersion != null ? initialReleaseInfo.ReleaseVersions[releaseVersion] : initialReleaseInfo.ReleaseVersions.OrderByDescending(version => version.Key).First().Value;
            foreach (var dependency in versionDeps)
            {
                //Traverse the dependency tree using recursion
                releaseInfos.AddRange(this.ResolveDependencies(dependency.PackageName, dependency.DependencyVersion));
            }
            return releaseInfos.Distinct();
        }

        public static string GetNugetDownload(ReleaseInfo releaseinfo, string version = "")
        {
            return $"https://www.nuget.org/api/v2/package/snowflake-snowball-{releaseinfo.PackageType.ToString()}-{releaseinfo.Name}/{version}";
        }
    }
}
