using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.JsonConverters;

namespace Snowflake.Platform
{
    [JsonConverter(typeof(PlatformInfoConverter))]
    public class PlatformInfo : IPlatformInfo
    {
        public PlatformInfo(string platformId, string name, IDictionary<string, string> metadata,
            IDictionary<string, string> fileTypes, IEnumerable<string> biosFiles, int maximumInputs)
        {
            this.PlatformID = platformId;
            this.Metadata = metadata;
            this.FileTypes = fileTypes;
            this.BiosFiles = biosFiles;
            this.MaximumInputs = maximumInputs;
            this.FriendlyName = name;
        }

        public string FriendlyName { get; }
        public string PlatformID { get; }
        public IDictionary<string, string> Metadata { get; set; }
        public IDictionary<string, string> FileTypes { get; }
        public IEnumerable<string> BiosFiles { get; }
        public int MaximumInputs { get; }
    }
}
