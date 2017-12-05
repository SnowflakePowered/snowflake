using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Caching;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Support.Caching.KeyedImageCache
{
    public class KeyedImageCacheComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IContentDirectoryProvider))]
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            var appdata = serviceContainer.Get<IContentDirectoryProvider>();
            var register = serviceContainer.Get<IServiceRegistrationProvider>();
            register.RegisterService<IKeyedImageCache>(new KeyedImageCache(appdata.ApplicationData));
        }
    }
}
