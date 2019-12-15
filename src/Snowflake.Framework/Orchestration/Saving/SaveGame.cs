using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Orchestration.Saving
{
    public class SaveGame : ISaveGame
    {
        public IReadOnlyDirectory SaveContents { get; }
        public DateTimeOffset CreatedTimestamp { get; }
        public Guid Guid { get; }
        public string SaveType { get; }
        public IEnumerable<string> Tags { get; }

        public SaveGame(IReadOnlyDirectory saveDirectory,
            DateTimeOffset createdTime,
            Guid saveGuid,
            string saveType,
            IEnumerable<string> tags)
        {
            this.SaveContents = saveDirectory;
            this.CreatedTimestamp = createdTime;
            this.Guid = saveGuid;
            this.SaveType = saveType;
            this.Tags = tags.ToList();
        }
    }
}
