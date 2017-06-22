using Snowflake.Configuration;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Utility;
using System.IO;

namespace Snowflake.Support.StoreProviders
{
    public class ConfigurationCollectionStoreComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        public void Compose(IServiceContainer serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            register.RegisterService<IConfigurationCollectionStore>(new SqliteConfigurationCollectionStore(new SqliteDatabase(Path.Combine(appdata.ApplicationData.FullName,
                "configurations.db"))));

        }
    }
}
