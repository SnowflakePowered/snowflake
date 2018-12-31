using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Configuration;
using Snowflake.Model.Database;
using Snowflake.Model.Records.Game;

namespace Snowflake.Model.Game.LibraryExtensions
{
    internal class GameConfigurationExtensionProvider : IGameConfigurationExtensionProvider
    {
        private ConfigurationCollectionStore CollectionStore { get; }

        public GameConfigurationExtensionProvider(ConfigurationCollectionStore collectionStore)
        {
            this.CollectionStore = collectionStore;
        }

        public void DeleteProfile(Guid valueCollectionGuid)
        {
            this.CollectionStore.DeleteConfiguration(valueCollectionGuid);
        }

        public IGameConfigurationExtension MakeExtension(IGameRecord record)
        {
            return new GameConfigurationExtension(record, this.CollectionStore);
        }

        public void UpdateValue(IConfigurationValue newValue)
        {
            this.CollectionStore.UpdateValue(newValue);
        }

        public void UpdateValue(Guid valueGuid, object newValue)
        {
            this.CollectionStore.UpdateValue(valueGuid, newValue);
        }

        public IConfigurationCollection<T>? GetProfile<T>(Guid valueCollectionGuid)
            where T: class, IConfigurationCollection<T>
        {
            return this.CollectionStore.GetConfiguration<T>(valueCollectionGuid);
        }

        IGameExtension IGameExtensionProvider.MakeExtension(IGameRecord record)
        {
            return this.MakeExtension(record);
        }

        public void UpdateProfile(IConfigurationCollection profile)
        {
            this.CollectionStore.UpdateConfiguration(profile);
        }
    }
}
