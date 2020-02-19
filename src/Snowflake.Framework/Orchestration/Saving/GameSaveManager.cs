using EnumsNET;
using Snowflake.Filesystem;
using Snowflake.Orchestration.Saving.SaveProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Orchestration.Saving
{
    internal class GameSaveManager : IGameSaveManager
    {
        public GameSaveManager(IDirectory saveRoot)
        {
            this.SaveRoot = saveRoot;
        }

        public IDirectory SaveRoot { get; }

        public ISaveProfile CreateProfile(string profileName, string saveType, SaveManagementStrategy managementStrategy)
        {
            var guid = Guid.NewGuid();
            var profileDirectory = this.SaveRoot.OpenDirectory($"{guid}-{managementStrategy}-{saveType}-{profileName}");
            return managementStrategy switch
            {
                SaveManagementStrategy.Replace =>  new ReplaceStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.Copy => new CopyStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.Diff => new DiffingStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.None => new NoneStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                _ => throw new NotImplementedException(),
            };
        }

        public void DeleteProfile(Guid guid)
        {
            this.SaveRoot.EnumerateDirectories().FirstOrDefault(d => d.Name.StartsWith(guid.ToString()))?.Delete();
        }

        private ISaveProfile? ParseProfile(IDirectory profileDirectory)
        {
            var guidParts = profileDirectory.Name[0..36];
            var nameParts = profileDirectory.Name[36..].Split("-", 3);
            if (nameParts.Length != 3) return null;
            if (!Guid.TryParseExact(guidParts, "D", out var guid)) return null;
            string saveType = nameParts[2];
            string profileName = nameParts[3];
            return Enums.Parse<SaveManagementStrategy>(nameParts[1], true) switch
            {
                SaveManagementStrategy.Replace => new ReplaceStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.Copy => new CopyStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.Diff => new DiffingStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                SaveManagementStrategy.None => new NoneStrategySaveProfile(guid, saveType, profileName, profileDirectory),
                _ => throw new NotImplementedException(),
            };
        }

        public ISaveProfile? GetProfile(Guid guid)
        {
            var profileDir = this.SaveRoot.EnumerateDirectories()
                .FirstOrDefault(d => d.Name.StartsWith(guid.ToString()));
            if (profileDir == null) return null;
            return this.ParseProfile(profileDir);
        }

        public IEnumerable<ISaveProfile> GetProfiles()
        {
            return this.SaveRoot.EnumerateDirectories().Select(this.ParseProfile).Where(d => d != null);
        }

        public IEnumerable<ISaveProfile> GetProfiles(string saveType)
        {
            return this.SaveRoot.EnumerateDirectories().Select(this.ParseProfile)
                .Where(d => d != null && d.SaveType == saveType);
        }
    }
}
