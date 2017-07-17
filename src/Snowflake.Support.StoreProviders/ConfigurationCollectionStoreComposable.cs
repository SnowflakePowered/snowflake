using Snowflake.Configuration;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.StoreProviders
{
    public class ConfigurationCollectionStoreComposable : IComposable
    {
        [ImportService(typeof(ISqliteDatabaseProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqlDb = serviceContainer.Get<ISqliteDatabaseProvider>();
            register.RegisterService<IConfigurationCollectionStore>(new SqliteConfigurationCollectionStore(sqlDb.CreateDatabase("configurations")));

        }
    }
}
