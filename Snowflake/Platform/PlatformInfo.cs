using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.Information;
using Snowflake.JsonConverters;

namespace Snowflake.Platform
{
    [JsonConverter(typeof(PlatformInfoConverter))]
    public class PlatformInfo : IPlatformInfo
    {
        public PlatformInfo(string platformId, string name, IDictionary<string, string> metadata,
            IEnumerable<string> fileExtensions, int maximumInputs)
        {
            this.PlatformID = platformId;
            this.Metadata = metadata;
            this.FileExtensions = fileExtensions;
            this.MaximumInputs = maximumInputs;
            this.FriendlyName = name;
        }

        public string FriendlyName { get; }
        public string PlatformID { get; }
        public IDictionary<string, string> Metadata { get; set; }
        public IEnumerable<string> FileExtensions { get; }
        public int MaximumInputs { get; }
    }
}
