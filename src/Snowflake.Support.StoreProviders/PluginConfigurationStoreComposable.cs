using Snowflake.Configuration;
using Snowflake.Extensibility.Configuration;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.StoreProviders
{
    public class PluginConfigurationStoreComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(ISqliteDatabaseProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqlDb = serviceContainer.Get<ISqliteDatabaseProvider>();
            register.RegisterService<IPluginConfigurationStore>(new SqlitePluginConfigurationStore(sqlDb.CreateDatabase("pluginconfig")));
        }
    }
}
