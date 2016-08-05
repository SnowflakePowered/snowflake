using System;

namespace Snowflake.Configuration
{
    public interface IConfigurationCollectionStore
    {
        void SetConfiguration(IConfigurationCollection collection, Guid gameRecord);
        T GetConfiguration<T>(Guid gameRecord) where T : IConfigurationCollection, new();
    }
}