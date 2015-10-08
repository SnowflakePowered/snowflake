using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Packaging.Snowball;
using NuGet;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
        [JsonProperty("dependencies")]
        public IList<Dependency> Dependencies { get; }
        [JsonProperty("packagetype")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType PackageType { get; }
        [JsonProperty("versions")]
        public IList<SemanticVersion> ReleaseVersions { get; }
        [JsonConstructor]
        public ReleaseInfo(string name, string description, IList<string> authors, IList<string> versions, IList<string> dependencies, PackageType packageType)
        {
            this.ReleaseVersions = versions.Select(version => new SemanticVersion(version)).ToList();
            this.Name = name;
            this.Description = description;
            this.Authors = authors;
            this.Dependencies = dependencies.Select(dependency => new Dependency(dependency)).ToList();
            this.PackageType = packageType;
        }

    }
}
