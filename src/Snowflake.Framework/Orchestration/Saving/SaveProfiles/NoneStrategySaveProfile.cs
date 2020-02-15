using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class NoneStrategySaveProfile : SaveProfile
    {
        public NoneStrategySaveProfile(Guid profileGuid, 
            string saveType, string profileName, IDirectory profileRoot) 
            : base(profileGuid, saveType, profileName, profileRoot)
        {
        }

        public override SaveManagementStrategy ManagementStrategy => SaveManagementStrategy.None;

        public override Task<ISaveGame> CreateSave(IDirectory saveContents)
        {
            return Task.FromResult<ISaveGame>(new NoneSaveGame(DateTimeOffset.UtcNow, Guid.NewGuid(), this.SaveType));
        }

        public override Task<ISaveGame> CreateSave(ISaveGame saveGame)
        {
            return Task.FromResult<ISaveGame>(new NoneSaveGame(DateTimeOffset.UtcNow, Guid.NewGuid(), this.SaveType));
        }

        public override ISaveGame? GetHeadSave()
        {
            return new NoneSaveGame(DateTimeOffset.UtcNow, Guid.NewGuid(), this.SaveType);
        }

        public override IEnumerable<ISaveGame> GetHistory()
        {
            return Enumerable.Empty<ISaveGame>();
        }
    }
}
