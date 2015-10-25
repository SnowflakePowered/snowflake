using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NuGet;
using Snowflake.Packaging.Snowball;

namespace Snowflake.Packaging.Publishing
{
    public class ReleaseInfo
    {
        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("description")]
        public string Description { get; }

        [JsonProperty("authors")]
        public IList<string> Authors { get; }

        [JsonProperty("tags")]
        public IList<string> Tags { get; }

        [JsonProperty("packagetype")]
        [JsonConverter(typeof (StringEnumConverter))]
        public PackageType PackageType { get; }

        [JsonProperty("versions")]
        public IDictionary<SemanticVersion, IList<Dependency>> ReleaseVersions { get; }

        [JsonConstructor]
        public ReleaseInfo(string name, string description, IList<string> authors, IList<string> tags,
            IDictionary<SemanticVersion, IList<Dependency>> versions, PackageType packageType)
        {
            this.ReleaseVersions = versions;
            this.Name = name;
            this.Description = description;
            this.Authors = authors;
            this.Tags = tags;
            this.PackageType = packageType;
        }

        public ReleaseInfo(PackageInfo packageInfo)
        {
            this.Name = packageInfo.Name;
            this.Description = packageInfo.Description;
            this.Authors = packageInfo.Authors;
            this.PackageType = packageInfo.PackageType;
            this.ReleaseVersions = new Dictionary<SemanticVersion, IList<Dependency>>
            {
                {packageInfo.Version, packageInfo.Dependencies}
            };
            this.Tags = packageInfo.Tags;
        }
    }
}