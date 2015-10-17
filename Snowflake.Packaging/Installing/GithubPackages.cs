using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Packaging.Publishing;
using Octokit;
using System.Net;
using Newtonsoft.Json;

namespace Snowflake.Packaging.Installing
{
    public static class GithubPackages
    {
        public static async Task<bool> PluginExistsInRepository(string packageId)
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            string pluginIndexFile = packageId + ".json";
            var masterContents = await gh.Repository.Content.GetAllContents("SnowflakePowered-Packages", "snowball-packages", "index/");
            return masterContents.Select(content => content.Name).Contains(pluginIndexFile);
        }

        public static async Task<ReleaseInfo> GetReleaseInfo(string packageId)
        {
            var gh = new GitHubClient(new ProductHeaderValue("snowball"));
            string pluginIndexFile = packageId + ".json";
            var masterContents = await gh.Repository.Content.GetAllContents("SnowflakePowered-Packages", "snowball-packages", "index/");
            var ghReleaseInfoContent = masterContents.First(content => content.Name == pluginIndexFile);
            string jsonFile = await new WebClient().DownloadStringTaskAsync(ghReleaseInfoContent.DownloadUrl);
            var releaseInfo = JsonConvert.DeserializeObject<ReleaseInfo>(jsonFile);
            return releaseInfo;
        }

        public static string GetNugetDownload(ReleaseInfo releaseinfo, string version = "")
        {
            return $"https://www.nuget.org/api/v2/package/snowflake-snowball-{releaseinfo.PackageType.ToString()}-{releaseinfo.Name}/{version}";
        }
    }
}
