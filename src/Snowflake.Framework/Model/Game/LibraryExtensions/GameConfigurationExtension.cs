using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Model.Database;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameConfigurationExtension : IGameConfigurationExtension
    {
        private IGameRecord GameRecord { get; }
        private ConfigurationCollectionStore ConfigurationStore { get; }

        public GameConfigurationExtension(IGameRecord gameRecord,
            ConfigurationCollectionStore collectionStore)
        {
            GameRecord = gameRecord;
            this.ConfigurationStore = collectionStore;
        }

        public void DeleteProfile(string sourceName, string profile)
        {
            this.ConfigurationStore
                .DeleteConfigurationForGame(this.GameRecord.RecordID, sourceName, profile);
        }

        public void DeleteProfile(string sourceName, Guid profile)
        {
            this.ConfigurationStore
                .DeleteConfigurationForGame(this.GameRecord.RecordID, sourceName, profile);
        }

        public IEnumerable<IGrouping<string, (string profileName, Guid collectionGuid)>> GetProfileNames()
        {
            return this.ConfigurationStore.GetProfileNames(this.GameRecord);
        }

        public IConfigurationCollection<T> CreateNewProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .CreateConfigurationForGame<T>(this.GameRecord, sourceName, profile);
        }

        public IConfigurationCollection<T>? GetProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .GetConfiguration<T>(this.GameRecord.RecordID, sourceName, profile);
        }

        public async Task DeleteProfileAsync(string sourceName, string profile)
        {
            await this.ConfigurationStore
                .DeleteConfigurationForGameAsync(this.GameRecord.RecordID, sourceName, profile);
        }

        public async Task DeleteProfileAsync(string sourceName, Guid profile)
        {
            await this.ConfigurationStore
                .DeleteConfigurationForGameAsync(this.GameRecord.RecordID, sourceName, profile);
        }

        public Task<IConfigurationCollection<T>> CreateNewProfileAsync<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .CreateConfigurationForGameAsync<T>(this.GameRecord, sourceName, profile);
        }

        public Task<IConfigurationCollection<T>?> GetProfileAsync<T>(string sourceName, string profile)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .GetConfigurationAsync<T>(this.GameRecord.RecordID, sourceName, profile);
        }

        public IConfigurationCollection<T>? GetProfile<T>(string sourceName, Guid collectionGuid)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore.GetConfiguration<T>(this.GameRecord.RecordID, sourceName, collectionGuid);
        }

        public Task<IConfigurationCollection<T>?> GetProfileAsync<T>(string sourceName, Guid collectionGuid)
            where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore.GetConfigurationAsync<T>(this.GameRecord.RecordID, sourceName, collectionGuid);
        }
    }
}
