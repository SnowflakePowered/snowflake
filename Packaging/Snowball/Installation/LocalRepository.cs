using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NuGet;
using Octokit;
using Snowball.Publishing;

namespace Snowball.Installation
{
    internal static class IEnumerableExtensions
    {
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> knownKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                    yield return element;
            }
        }
    }

    public class LocalRepository
    {
        public string AppDataDirectory { get; }
        public string RepositoryOrg { get; }
        public string RepositoryName { get; }
        private string ArchivePath { get; }

        private string CachedRepoHash
        {
            get { return File.ReadAllText(Path.Combine(this.AppDataDirectory, ".repohash")); }
            set { File.WriteAllText(Path.Combine(this.AppDataDirectory, ".repohash"), value); }
        }

        private ZipArchive RepositoryZip { get; set; }

        public LocalRepository(string appDataDirectory,
            string repositorySlug = "SnowflakePowered-Packages/snowball-packages")
        {
            this.AppDataDirectory = appDataDirectory;
            this.RepositoryOrg = repositorySlug.Split('/')[0];
            this.RepositoryName = repositorySlug.Split('/')[1];
            this.ArchivePath = Path.Combine(this.AppDataDirectory, "snowball.repo");
            if (File.Exists(this.ArchivePath)) this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));
        }

        public async Task<bool> UpdatedRequired()
        {
            string cachedRepoHash = this.CachedRepoHash;
            if (string.IsNullOrWhiteSpace(cachedRepoHash) || this.RepositoryZip == null)
                return true;
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            var _refs = await gh.GitDatabase.Reference.GetAll(this.RepositoryOrg, this.RepositoryName);
            string remoteRepoHash = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
            return remoteRepoHash != cachedRepoHash;
        }

        public async Task UpdateRepository()
        {
            using (var downloader = new WebClient())
            {
                this.RepositoryZip?.Dispose();
                if (File.Exists(this.ArchivePath)) File.Delete(this.ArchivePath);
                Console.WriteLine($"Updating repository cache from {this.RepositoryOrg}/{this.RepositoryName}");
                await
                    downloader.DownloadFileTaskAsync(
                        $"https://github.com/{this.RepositoryOrg}/{this.RepositoryName}/archive/master.zip",
                        this.ArchivePath);
                var gh = new GitHubClient(new ProductHeaderValue("snowball"));
                var _refs = await gh.GitDatabase.Reference.GetAll(this.RepositoryOrg, this.RepositoryName);
                string remoteRepoHash = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
                this.CachedRepoHash = remoteRepoHash;
                this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));
            }
        }

        public bool PluginExistsInRepository(string packageId)
        {
            return
                this.RepositoryZip?.Entries.Any(entry => entry.Name.StartsWith($"{packageId}.") && entry.Name.EndsWith(".rel.json")) ??
                false;
        }

        public ReleaseInfo GetReleaseInfo(string packageId)
        {
            var zipArchiveStream =
              this.RepositoryZip.Entries.FirstOrDefault(entry => entry.Name.StartsWith($"{packageId}.") && entry.Name.EndsWith(".rel.json"))?
                  .Open();

            return zipArchiveStream != null ? JsonConvert.DeserializeObject<ReleaseInfo>(new StreamReader(zipArchiveStream).ReadToEnd()) : null;
        }

        public IEnumerable<ReleaseInfo> GetAllReleases()
        {
            return this.RepositoryZip?.Entries.Where(releaseEntry => releaseEntry.Name.EndsWith(".rel.json"))
                .Select(entry => JsonConvert.DeserializeObject<ReleaseInfo>(new StreamReader(entry.Open()).ReadToEnd()));
        }

        public bool CheckPublishUpdate(ReleaseInfo releaseInfo)
        {
            return this.RepositoryZip.Entries.FirstOrDefault(
                entry => StringComparer.InvariantCultureIgnoreCase.Equals(entry.Name, $"{releaseInfo.Name}.{releaseInfo.PackageType}.rel.json".ToLowerInvariant())) !=
                   null;
        }

        public ReleaseInfo MergeReleaseVersions(ReleaseInfo releaseInfo)
        {
            if (!this.CheckPublishUpdate(releaseInfo)) return releaseInfo;
            var oldReleaseInfo = this.GetReleaseInfo(releaseInfo.Name);
            foreach (var version in releaseInfo.ReleaseVersions.Where(version => !oldReleaseInfo.ReleaseVersions.ContainsKey(version.Key)))
            {
                oldReleaseInfo.ReleaseVersions.Add(version);
            }
            return oldReleaseInfo;
        }

        public IEnumerable<Tuple<ReleaseInfo, SemanticVersion>> ResolveDependencies(string packageId,
            SemanticVersion releaseVersion = null)
        {
            IList<Tuple<ReleaseInfo, SemanticVersion>> releaseInfos = new List<Tuple<ReleaseInfo, SemanticVersion>>();
            var initialReleaseInfo = this.GetReleaseInfo(packageId);
            if (initialReleaseInfo == null) return null;
            releaseInfos.Add(Tuple.Create(initialReleaseInfo, releaseVersion));
            var versionDeps = releaseVersion != null
                ? initialReleaseInfo.ReleaseVersions[releaseVersion]
                : initialReleaseInfo.ReleaseVersions.OrderByDescending(version => version.Key).First().Value;
            foreach (var dependency in versionDeps)
            {
                //Traverse the dependency tree using recursion
                releaseInfos.AddRange(this.ResolveDependencies(dependency.PackageName, dependency.DependencyVersion));
            }
            return releaseInfos.DistinctBy(key => key.Item1.Name);
        }

        public IEnumerable<Tuple<ReleaseInfo, SemanticVersion>> ResolveDependencies(IEnumerable<string> releaseStrings)
        {
            var _installList = new List<Tuple<ReleaseInfo, SemanticVersion>>();
            foreach (string installPackage in releaseStrings)
            {
                string packageName;
                SemanticVersion packageVersion = null;

                if (installPackage.Contains('@'))
                {
                    packageName = installPackage.Split('@')[1];
                    packageVersion = new SemanticVersion(installPackage.Split('@')[2]);
                }
                else
                    packageName = installPackage;
                _installList.AddRange(this.ResolveDependencies(packageName, packageVersion));
            }
            return _installList.DistinctBy(key => key.Item1.Name);
        }

        public static string GetNugetDownload(ReleaseInfo releaseinfo, string version = "")
        {
            return
                $"https://www.nuget.org/api/v2/package/snowflake-snowball-{releaseinfo.PackageType}-{releaseinfo.Name}/{version}";
        }

        public static string GetNugetPage(ReleaseInfo releaseinfo, string version = "")
        {
            return
                $"https://www.nuget.org/packages/snowflake-snowball-{releaseinfo.PackageType}-{releaseinfo.Name}/{version}";
        }
    }
}