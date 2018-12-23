using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.JsonConverters;
using Snowflake.Model.Game;

namespace Snowflake.Platform
{
    [JsonConverter(typeof(PlatformInfoConverter))]
    public class PlatformInfo : IPlatformInfo
    {
        public PlatformInfo(string platformId, string name, IDictionary<string, string> metadata,
            IDictionary<string, string> fileTypes, IEnumerable<IBiosFile> biosFiles, int maximumInputs)
        {
            this.PlatformID = platformId;
            this.Metadata = metadata;
            this.FileTypes = fileTypes;
            this.BiosFiles = biosFiles;
            this.MaximumInputs = maximumInputs;
            this.FriendlyName = name;
        }

        /// <inheritdoc/>
        public string FriendlyName { get; }

        /// <inheritdoc/>
        public string PlatformID { get; }

        /// <inheritdoc/>
        public IDictionary<string, string> Metadata { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> FileTypes { get; }

        /// <inheritdoc/>
        public IEnumerable<IBiosFile> BiosFiles { get; }

        /// <inheritdoc/>
        public int MaximumInputs { get; }
    }
}
