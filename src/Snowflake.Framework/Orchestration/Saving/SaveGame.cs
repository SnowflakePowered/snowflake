using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    public abstract class SaveGame : ISaveGame
    {
        public DateTimeOffset CreatedTimestamp { get; }
        public Guid Guid { get; }
        public string SaveType { get; }

        protected SaveGame(DateTimeOffset createdTime,
            Guid saveGuid,
            string saveType)
        {
            this.CreatedTimestamp = createdTime;
            this.Guid = saveGuid;
            this.SaveType = saveType;
        }

        public abstract Task ExtractSave(IDirectory outputDirectory);
    }
}
