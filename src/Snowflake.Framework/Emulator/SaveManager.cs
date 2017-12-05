using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Records.File;
using Snowflake.Records.Game;

namespace Snowflake.Emulator
{
    public class SaveManager : ISaveManager
    {
        private string SavegameDirectory { get; }
        public SaveManager(string appdataDirectory)
        {
            this.SavegameDirectory = Path.Combine(appdataDirectory, "saves");
        }

        /// <inheritdoc/>
        public string GetSaveDirectory(string saveType, Guid gameGuid, int slot)
        {
            if (slot < 0)
            {
                return this.GetSharedSaveDirectory(saveType); // if the slot is less than 0, return the shared save directory
            }

            string saveDirectory = Path.Combine(this.SavegameDirectory, saveType, gameGuid.ToString(), slot.ToString());
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            File.WriteAllText(Path.Combine(saveDirectory, ".modified"), DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString());
            return saveDirectory;
        }

        /// <inheritdoc/>
        public DateTimeOffset GetLastModified(string saveType, Guid gameGuid, int slot)
        {
            string timestamp = Path.Combine(this.SavegameDirectory, saveType, gameGuid.ToString(), slot.ToString(), ".modified");
            return File.Exists(timestamp)
                ? DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(File.ReadAllText(timestamp)))
                : DateTimeOffset.UtcNow;
        }

        /// <inheritdoc/>
        public string GetSharedSaveDirectory(string saveType)
        {
            string saveDirectory = Path.Combine(this.SavegameDirectory, saveType, "shared");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            return saveDirectory;
        }

        /// <inheritdoc/>
        public IEnumerable<int> GetUsedSlots(string saveType, Guid gameGuid)
        {
            string saveDirectory = Path.Combine(this.SavegameDirectory, saveType, gameGuid.ToString());
            if (!Directory.Exists(saveDirectory))
            {
                yield break;
            }

            int slot = 0;
            foreach (string directory in Directory.GetDirectories(saveDirectory)
                .Where(d => int.TryParse(Path.GetDirectoryName(d), out slot)))
            {
                yield return slot;
            }
        }
    }
}
