using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using semver.tools;

namespace Snowball.Installation
{
    internal static class EnumerableExtensions
    {
        internal static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }

    public class LocalRepository
    {
        public string AppDataDirectory { get; }
        public string RepositoryOrg { get; }
        public string RepositoryName { get; }
        private string ArchivePath { get; }
        private ZipArchive RepositoryZip { get; set; }

        public LocalRepository(string appDataDirectory,
            string repositorySlug = "SnowflakePowered-Packages/snowball-packages")
        {
            this.AppDataDirectory = appDataDirectory;
            this.RepositoryOrg = repositorySlug.Split('/')[0];
            this.RepositoryName = repositorySlug.Split('/')[1];
            this.ArchivePath = Path.Combine(this.AppDataDirectory, "snowball.repo");
            this.UpdateRepository();

        }

        public bool UpdatedRequired()
        {
            try
            {


                if (!File.Exists(this.ArchivePath))
                    return true;
                var oldRepository = File.OpenRead(this.ArchivePath);
                var newRepository = this.GetRepository();
                bool updateRequired =
                    !String.Equals(LocalRepository.HashMD5(oldRepository), LocalRepository.HashMD5(newRepository));
                oldRepository.Close();
                oldRepository.Dispose();
                return updateRequired;
            }
            catch
            {
                return false; 
            }
    }

        public FileStream GetRepository()
        {
            using (var downloader = new WebClient())
            {
                string tempPath = Path.GetTempFileName();
                downloader.DownloadFile($"https://github.com/{this.RepositoryOrg}/{this.RepositoryName}/archive/master.zip", tempPath);
                return File.OpenRead(tempPath);
            }
        }
        public void UpdateRepository(Stream newRepository = null)
        {
            if (!this.UpdatedRequired())
            {
                this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));
                return;
            }
            Console.WriteLine($"Updating repository cache from {this.RepositoryOrg}/{this.RepositoryName}");
            if (File.Exists(this.ArchivePath)) File.Delete(this.ArchivePath);
            this.RepositoryZip?.Dispose();
            var cachedRepository = File.Open(this.ArchivePath, FileMode.Create, FileAccess.ReadWrite);
            newRepository = newRepository ?? this.GetRepository();
            this.RepositoryZip = new ZipArchive(newRepository);
            newRepository?.CopyTo(cachedRepository);
            cachedRepository.Close();
            cachedRepository.Dispose();
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
            foreach (var version in oldReleaseInfo.ReleaseVersions.Where(version => !releaseInfo.ReleaseVersions.ContainsKey(version.Key)))
            {
               releaseInfo.ReleaseVersions.Add(version);
            }

            return releaseInfo;
        }

        public IEnumerable<Tuple<ReleaseInfo, SemanticVersion>> ResolveDependencies(string packageId,
            SemanticVersion releaseVersion = null)
        {
            var releaseInfos = new List<Tuple<ReleaseInfo, SemanticVersion>>();
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
                    packageVersion = SemanticVersion.Parse(installPackage.Split('@')[2]);
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

        private static string HashMD5(Stream contents)
        {
            using (var md5 = MD5.Create())
            {
                return
                    BitConverter.ToString(md5.ComputeHash(contents));
            }
        }
    }
}