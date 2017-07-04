using Snowflake.Loader;
using Snowflake.Persistence;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Utility;
using System.IO;

namespace Snowflake.Support.StoreProviders
{
    public class GameLibraryComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(ISqliteDatabaseProvider))]
        public void Compose(IModule composableModule, IServiceProvider serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var sqlDb = serviceContainer.Get<ISqliteDatabaseProvider>();
            register.RegisterService<IGameLibrary>(new SqliteGameLibrary(sqlDb.CreateDatabase("games")));

        }
    }
}
