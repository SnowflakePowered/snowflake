using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class CopyingSaveGame : SaveGame
    {
        public CopyingSaveGame(DateTimeOffset createdTime, 
            Guid saveGuid,
            string saveType,
            IDirectory contentDirectory) 
            : base(createdTime, saveGuid, saveType)
        {
            this.ContentDirectory = contentDirectory;
        }

        private IDirectory ContentDirectory { get; }

        public override async Task ExtractSave(IDirectory outputDirectory)
        {
            await foreach (var _ in outputDirectory.CopyFromDirectory(this.ContentDirectory, true)) { };
        }
    }
}
