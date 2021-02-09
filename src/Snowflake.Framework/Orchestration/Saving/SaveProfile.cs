using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving
{
    internal abstract class SaveProfile : ISaveProfile
    {
        public SaveProfile(Guid profileGuid, string saveType, string profileName, IDirectory profileRoot)
        {
            this.Guid = profileGuid;
            this.ProfileName = profileName;
            this.ProfileRoot = profileRoot;
            this.SaveType = saveType;
        }

        public Guid Guid { get; }

        public string SaveType { get; }

        public string ProfileName { get; }

        protected IDirectory ProfileRoot { get; }

        public abstract SaveManagementStrategy ManagementStrategy { get; }

        public abstract Task<ISaveGame> CreateSave(IReadOnlyDirectory saveContents);

        public abstract Task<ISaveGame> CreateSave(ISaveGame saveGame);

        public abstract ISaveGame? GetHeadSave();

        public abstract IEnumerable<ISaveGame> GetHistory();

        public abstract void ClearHistory();
    }
}
