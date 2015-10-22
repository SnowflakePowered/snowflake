using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Net;
using Octokit;

namespace Snowflake.Packaging.Installing
{
    public class LocalRepository
    {
        public string AppDataDirectory { get; }
        public string RepositoryOrg{ get; }
        public string RepositoryName { get; }
        public string ArchivePath { get; }
        public ZipArchive RepositoryZip { get; set; }
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
            if (String.IsNullOrWhiteSpace(Properties.Settings.Default.cachedRepoHash)) return true;
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            var _refs = await gh.GitDatabase.Reference.GetAll(this.RepositoryOrg, this.RepositoryName);
            string remoteRepoHash = _refs.First(branch => branch.Ref == "refs/heads/master").Object.Sha;
            return remoteRepoHash == Properties.Settings.Default.cachedRepoHash;
        }

        public async Task UpdateRepository()
        {
            using(var downloader = new WebClient())
            {
                this.RepositoryZip?.Dispose();
                if (File.Exists(this.ArchivePath)) File.Delete(this.ArchivePath);
                Console.WriteLine($"Updating repository cache from {this.RepositoryOrg}/{this.RepositoryName}");
                await downloader.DownloadFileTaskAsync($"https://github.com/{this.RepositoryOrg}/{this.RepositoryName}/archive/master.zip", this.ArchivePath);
                this.RepositoryZip = new ZipArchive(File.OpenRead(this.ArchivePath));
            }
        }
    }
}
