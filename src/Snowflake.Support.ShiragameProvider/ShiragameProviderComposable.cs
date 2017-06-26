using Snowflake.Loader;
using Snowflake.Persistence;
using Snowflake.Scraper.Shiragame;
using Snowflake.Services;
using System;
using System.IO;

namespace Snowflake.Support.ShiragameProvider
{
    public class ShiragameProviderComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, IServiceContainer serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var shiragameDb = new SqliteDatabase(Path.Combine(composableModule.ContentsDirectory.FullName, "shiragame.db"));
            register.RegisterService<IShiragameProvider>(new ShiragameProvider(shiragameDb));
        }
    }
}
