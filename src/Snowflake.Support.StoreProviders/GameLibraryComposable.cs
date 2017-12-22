using System.IO;
using Snowflake.Loader;
using Snowflake.Persistence;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Services;

namespace Snowflake.Support.StoreProviders
{
    public class GameLibraryComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ISqliteDatabaseProvider))]
        public void Compose(IModule composableModule, IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqlDb = serviceContainer.Get<ISqliteDatabaseProvider>();
            register.RegisterService<IGameLibrary>(new SqliteGameLibrary(sqlDb.CreateDatabase("games")));
        }
    }
}
