using Snowflake.Loader;
using Snowflake.Records.Game;
using Snowflake.Services;
using Snowflake.Utility;
using System.IO;

namespace Snowflake.Support.StoreProviders
{
    public class GameLibraryComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        [ImportService(typeof(IContentDirectoryProvider))]
        public void Compose(IServiceContainer serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            register.RegisterService<IGameLibrary>(new SqliteGameLibrary(new SqliteDatabase(Path.Combine(appdata.ApplicationData.FullName, "games.db"))));

        }
    }
}
