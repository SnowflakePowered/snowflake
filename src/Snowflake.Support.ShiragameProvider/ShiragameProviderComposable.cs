using System;
using System.IO;
using Snowflake.Loader;
using Snowflake.Persistence;
using Snowflake.Scraper.Shiragame;
using Snowflake.Services;

namespace Snowflake.Support.ShiragameProvider
{
    public class ShiragameProviderComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            var shiragameDb = new SqliteDatabase(Path.Combine(composableModule.ContentsDirectory.FullName, "shiragame.db"));
            register.RegisterService<IShiragameProvider>(new ShiragameProvider(shiragameDb));
        }
    }
}
