using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class ReplaceStrategySaveProfile : SaveProfile
    {
        private static readonly string DateFormat = "yyyy-MM-dd.HH-mm-ss";

        public ReplaceStrategySaveProfile(Guid profileGuid, 
            string saveType, string profileName, IDirectory profileRoot) 
            : base(profileGuid, saveType, profileName, profileRoot)
        {
            var saveManifest = this.ProfileRoot.OpenFile("profile");
            if (!saveManifest.Created)
            {
                saveManifest.WriteAllText($"{profileName}{Environment.NewLine}{nameof(SaveManagementStrategy.Replace)}");
            }
        }

        public override SaveManagementStrategy ManagementStrategy => SaveManagementStrategy.Replace;

        public async override Task<ISaveGame> CreateSave(IReadOnlyDirectory saveContents)
        {
            var oldLatestFile = this.ProfileRoot.OpenFile("latest");

            // todo decide on some way to organize these saves.
            var newGuid = Guid.NewGuid();
            var saveName = $"{DateTimeOffset.UtcNow.ToString(DateFormat)}-{newGuid}";
            var saveDirectory = this.ProfileRoot.OpenDirectory(saveName);
            var contentDirectory = saveDirectory.OpenDirectory("content");

            // Copy it twice, once for backup, once for head.
            await foreach (var _ in contentDirectory.CopyFromDirectory(saveContents, true)) { }

            if (oldLatestFile.Created)
            {
                var oldLatestName = oldLatestFile.ReadAllText();
                var oldLatest = this.ProfileRoot.OpenDirectory(oldLatestName);
                // Delete the old head
                oldLatest.Delete();
            }
            this.ProfileRoot.OpenFile("latest").WriteAllText(saveName, Encoding.UTF8);
            return this.GetSave(saveDirectory);
        }

        public override async Task<ISaveGame> CreateSave(ISaveGame saveGame)
        {
            var oldLatestFile = this.ProfileRoot.OpenFile("latest");

            // todo decide on some way to organize these saves.
            var newGuid = Guid.NewGuid();
            var saveName = $"{DateTimeOffset.UtcNow.ToString(DateFormat)}-{newGuid}";
            var saveDirectory = this.ProfileRoot.OpenDirectory(saveName);
            var contentDirectory = saveDirectory.OpenDirectory("content");

            // Copy it twice, once for backup, once for head.
            await saveGame.ExtractSave(contentDirectory);

            // Delete the old head
            if (oldLatestFile.Created)
            {
                var oldLatestName = oldLatestFile.ReadAllText();
                var oldLatest = this.ProfileRoot.OpenDirectory(oldLatestName);
                // Delete the old head
                oldLatest.Delete();
            }
            this.ProfileRoot.OpenFile("latest").WriteAllText(saveName, Encoding.UTF8);

            return this.GetSave(saveDirectory);
        }

        private ISaveGame GetSave(IDirectory internalSaveDir)
        {
            string name = internalSaveDir.Name;
            var date = DateTimeOffset.ParseExact(name[0..DateFormat.Length], DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
            var guid = Guid.ParseExact(name[(DateFormat.Length + 1)..], "D");
            return new CopyingSaveGame(date, guid, this.SaveType, internalSaveDir.OpenDirectory("content"));
        }

        public override ISaveGame? GetHeadSave()
        {
            var latest = this.ProfileRoot.OpenFile("latest").ReadAllText();
            if (latest.Length < DateFormat.Length) return null;
            var saveDirectory = this.ProfileRoot.OpenDirectory(latest);
            return this.GetSave(saveDirectory);
        }

        public override IEnumerable<ISaveGame> GetHistory()
        {
            var save = this.GetHeadSave();
            if (save == null) yield break;
            yield return save;
        }
        public override void ClearHistory()
        {
            // no history to clear
            return;
        }
    }
}
