using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class EmptySaveGame : SaveGame
    {
        public EmptySaveGame(DateTimeOffset createdTime, Guid saveGuid, string saveType) 
            : base(createdTime, saveGuid, saveType)
        {
        }

        public override Task ExtractSave(IIndelibleDirectory outputDirectory)
        {
            return Task.CompletedTask;
        }
    }
}
