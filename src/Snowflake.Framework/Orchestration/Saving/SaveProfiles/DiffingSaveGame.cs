using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal class DiffingSaveGame : SaveGame
    {
        public DiffingSaveGame(DateTimeOffset createdTime, Guid saveGuid, string saveType) 
            : base(createdTime, saveGuid, saveType)
        {
        }

        public override Task ExtractSave(IDirectory outputDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
