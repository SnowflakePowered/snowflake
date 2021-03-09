using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Internal;
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

        public void DeleteProfile(string sourceName, Guid profile)
        {
            this.ConfigurationStore
                .DeleteConfigurationForGame(this.GameRecord.RecordID, sourceName, profile);
        }

        public IEnumerable<IGrouping<string, (string profileName, Guid collectionGuid)>> GetProfileNames()
        {
            return this.ConfigurationStore.GetProfileNames(this.GameRecord);
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T> CreateNewProfile<T>(string sourceName, string profile)
            where T : class, IConfigurationCollectionTemplate
        {
            return this.ConfigurationStore
                .CreateConfigurationForGame<T>(this.GameRecord, sourceName, profile);
        }

        public async Task DeleteProfileAsync(string sourceName, Guid profile)
        {
            await this.ConfigurationStore
                .DeleteConfigurationForGameAsync(this.GameRecord.RecordID, sourceName, profile);
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public Task<IConfigurationCollection<T>> CreateNewProfileAsync<T>(string sourceName, string profile)
            where T : class, IConfigurationCollectionTemplate
        {
            return this.ConfigurationStore
                .CreateConfigurationForGameAsync<T>(this.GameRecord, sourceName, profile);
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T>? GetProfile<T>(string sourceName, Guid collectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            return this.ConfigurationStore.GetConfiguration<T>(this.GameRecord.RecordID, sourceName, collectionGuid);
        }

        [GenericTypeAcceptsConfigurationCollection(0)]
        public Task<IConfigurationCollection<T>?> GetProfileAsync<T>(string sourceName, Guid collectionGuid)
            where T : class, IConfigurationCollectionTemplate
        {
            return this.ConfigurationStore.GetConfigurationAsync<T>(this.GameRecord.RecordID, sourceName, collectionGuid);
        }
    }
}
