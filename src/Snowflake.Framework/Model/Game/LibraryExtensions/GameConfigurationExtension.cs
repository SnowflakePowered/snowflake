using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Model.Database;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameConfigurationExtension : IGameConfigurationExtension
    {
        public IGameRecord GameRecord { get; }
        public ConfigurationCollectionStore ConfigurationStore { get; }

        public GameConfigurationExtension(IGameRecord gameRecord, 
            ConfigurationCollectionStore collectionStore)
        {
            GameRecord = gameRecord;
            this.ConfigurationStore = collectionStore;
        }

        public void DeleteProfile(string sourceName, string profile)
        {
            this.ConfigurationStore
                .DeleteConfigurationForGame(this.GameRecord.RecordId, sourceName, profile);
        }

        public IEnumerable<IGrouping<string, string>> GetProfileNames()
        {
            return this.ConfigurationStore.GetProfileNames(this.GameRecord);
        }

        public IConfigurationCollection<T> CreateNewProfile<T>(string sourceName, string profile)
            where T: class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .CreateConfigurationForGame<T>(this.GameRecord, sourceName, profile);
        }

        public IConfigurationCollection<T>? GetProfile<T>(string sourceName, string profile)
             where T : class, IConfigurationCollection<T>
        {
            return this.ConfigurationStore
                .GetConfiguration<T>(this.GameRecord.RecordId, sourceName, profile);
        }
    }
}
