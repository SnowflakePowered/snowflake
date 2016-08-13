using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.Savegames
{
    public class SavegameStore
    {
        private string SavegameDirectory { get; }
        public SavegameStore(string appdataDirectory, IGameLibrary gameLibrary)
        {
            this.SavegameDirectory = Path.Combine(appdataDirectory, "saves");
        }

        public IFileRecord StoreSavegame(string savegamePath, string savegameType, IGameRecord gameRecord)
        {
            var savegameGuid = Guid.NewGuid();
            if (
                !Directory.Exists(Path.Combine(this.SavegameDirectory, savegameType, gameRecord.Guid.ToString(),
                    savegameGuid.ToString()))) ;
            ;            return null;
            return null;
        }
    }
}
