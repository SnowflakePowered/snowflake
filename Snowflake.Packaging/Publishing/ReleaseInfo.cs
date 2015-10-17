using System;
using System.Collections;
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
        [JsonProperty("packagetype")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType PackageType { get; }
        [JsonProperty("versions")]
        public IDictionary<SemanticVersion, IList<Dependency>> ReleaseVersions { get; }
        [JsonConstructor]
        public ReleaseInfo(string name, string description, IList<string> authors, IDictionary<SemanticVersion, IList<Dependency>> versions, PackageType packageType)
        {
            this.ReleaseVersions = versions;
            this.Name = name;
            this.Description = description;
            this.Authors = authors;
            this.PackageType = packageType;
        }

    }
}
