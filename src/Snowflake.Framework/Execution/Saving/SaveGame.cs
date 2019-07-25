using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Execution.Saving
{
    public class SaveGame
    {
        public IDirectory SaveContents { get; }
        public DateTimeOffset CreatedTimestamp { get; }
        public Guid Guid { get; }
        public string SaveType { get; }
        public IEnumerable<string> Tags { get; }

        public SaveGame(IDirectory saveDirectory,
            DateTimeOffset createdTime,
            Guid saveGuid,
            string saveType,
            IEnumerable<string> tags)
        {
            this.SaveContents = saveDirectory.OpenDirectory("savecontents");
            this.CreatedTimestamp = createdTime;
            this.Guid = saveGuid;
            this.SaveType = saveType;
            this.Tags = tags.ToList();
        }
    }
}
