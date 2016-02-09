using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using semver.tools;
using Snowball.Packaging;
using Snowball.Packaging.Packagers;

namespace Snowball.Installation
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

        [JsonProperty("pub")]
        public string PublicKey { get; }

        [JsonConstructor]
        public ReleaseInfo(string name, string description, IList<string> authors, IList<string> tags,
            IDictionary<SemanticVersion, IList<Dependency>> versions, PackageType packageType, string publicKey)
        {
            this.ReleaseVersions = versions;
            this.Name = name;
            this.Description = description;
            this.Authors = authors;
            this.Tags = tags;
            this.PackageType = packageType;
            this.PublicKey = publicKey;
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
        public ReleaseInfo(PackageInfo packageInfo, PackageKeyPair keyPair)
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
            this.PublicKey = keyPair.PublicKey;
        }
    }
}