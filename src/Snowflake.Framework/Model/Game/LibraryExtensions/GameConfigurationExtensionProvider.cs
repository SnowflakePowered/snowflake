using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Internal;
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

        [GenericTypeAcceptsConfigurationCollection(0)]
        public IConfigurationCollection<T>? GetProfile<T>(Guid valueCollectionGuid)
            where T : class, IConfigurationCollectionTemplate
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

        Task<IConfigurationCollection<T>?> IGameConfigurationExtensionProvider.GetProfileAsync<T>(Guid valueCollectionGuid)
        {
            return this.CollectionStore.GetConfigurationAsync<T>(valueCollectionGuid);
        }

        public Task DeleteProfileAsync(Guid valueCollectionGuid)
        {
            return this.CollectionStore.DeleteConfigurationAsync(valueCollectionGuid);
        }

        public Task UpdateValueAsync(IConfigurationValue newValue)
        {
            return this.CollectionStore.UpdateValueAsync(newValue);
        }

        public Task UpdateValueAsync(Guid valueGuid, object newValue)
        {
            return this.CollectionStore.UpdateValueAsync(valueGuid, newValue);
        }

        public Task UpdateProfileAsync(IConfigurationCollection profile)
        {
            return this.CollectionStore.UpdateConfigurationAsync(profile);
        }

        public Guid GetOwningValueCollection(Guid valueGuid)
        {
            return this.CollectionStore.GetOwningValueCollection(valueGuid);
        }

        public Task<Guid> GetOwningValueCollectionAsync(Guid valueGuid)
        {
            return this.CollectionStore.GetOwningValueCollectionAsync(valueGuid);
        }

        public Task<IConfigurationValue> GetValueAsync(Guid valueGuid)
        {
            return this.CollectionStore.GetValueAsync(valueGuid);
        }

        public IConfigurationValue GetValue(Guid valueGuid)
        {
            return this.CollectionStore.GetValue(valueGuid);
        }
    }
}
