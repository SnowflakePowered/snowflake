using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.JsonConverters;
using Snowflake.Model.Game;

namespace Snowflake.Model.Game
{
    [JsonConverter(typeof(PlatformInfoConverter))]
    internal class PlatformInfo : IPlatformInfo
    {
        internal PlatformInfo(string platformId, string name, IDictionary<string, string> metadata,
            IDictionary<string, string> fileTypes, IEnumerable<ISystemFile> biosFiles, int maximumInputs)
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
        public PlatformId PlatformID { get; }

        /// <inheritdoc/>
        public IDictionary<string, string> Metadata { get; set; }

        /// <inheritdoc/>
        public IDictionary<string, string> FileTypes { get; }

        /// <inheritdoc/>
        public IEnumerable<ISystemFile> BiosFiles { get; }

        /// <inheritdoc/>
        public int MaximumInputs { get; }
    }
}
