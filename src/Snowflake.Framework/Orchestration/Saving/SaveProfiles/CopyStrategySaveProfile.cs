using Snowflake.Filesystem;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Saving.SaveProfiles
{
    internal sealed class CopyStrategySaveProfile : SaveProfile
    {
        private static readonly string DateFormat = "yyyy-MM-dd.HH-mm-ss";

        public CopyStrategySaveProfile(Guid profileGuid, 
            string saveType, string profileName, IIndelibleDirectory profileRoot) 
            : base(profileGuid, saveType, profileName, profileRoot)
        {
            var saveManifest = this.ProfileRoot.OpenFile("profile");
            if (!saveManifest.Created)
            {
                saveManifest.WriteAllText($"{profileName}{Environment.NewLine}{nameof(SaveManagementStrategy.Copy)}");
            }
        }

        public override SaveManagementStrategy ManagementStrategy => SaveManagementStrategy.Copy;

        public async override Task<ISaveGame> CreateSave(IIndelibleDirectory saveContents)
        {
            // todo decide on some way to organize these saves.
            var newGuid = Guid.NewGuid();
            var saveName = $"{DateTimeOffset.UtcNow.ToString(DateFormat)}-{newGuid}";
            var saveDirectory = this.ProfileRoot.OpenDirectory(saveName);
            var contentDirectory = saveDirectory.OpenDirectory("content");

            await foreach (var _ in contentDirectory.CopyFromDirectory(saveContents, true)) { };

            this.ProfileRoot.OpenFile("latest").WriteAllText(saveName, Encoding.UTF8);
            return this.GetSave(saveDirectory)!;
        }

        public override async Task<ISaveGame> CreateSave(ISaveGame saveGame)
        {
            var newGuid = Guid.NewGuid();
            var saveName = $"{DateTimeOffset.UtcNow.ToString(DateFormat)}-{newGuid}";
            var saveDirectory = this.ProfileRoot.OpenDirectory(saveName);
            var contentDirectory = saveDirectory.OpenDirectory("content");

            await saveGame.ExtractSave(contentDirectory);

            this.ProfileRoot.OpenFile("latest").WriteAllText(saveName, Encoding.UTF8);
            return this.GetSave(saveDirectory)!;
        }

        private ISaveGame? GetSave(IDirectory internalSaveDir)
        {
            string name = internalSaveDir.Name;
            if (!DateTimeOffset.TryParseExact(name[0..DateFormat.Length], DateFormat, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal, out var date)) return null;
            if (!Guid.TryParseExact(name[(DateFormat.Length + 1)..], "D", out var guid)) return null;
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
            return this.ProfileRoot.EnumerateDirectories().Select(this.GetSave).Where(s => s != null);
        }

        public override void ClearHistory()
        {
            var latest = this.ProfileRoot.OpenFile("latest").ReadAllText();
            foreach (var toDelete in this.ProfileRoot.EnumerateDirectories().Where(d => d.Name != latest))
            {
                toDelete.Delete();
            }
        }
    }
}
