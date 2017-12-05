using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;

namespace Snowflake.Services
{
    public class StoneProviderComposer : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IStoneProvider>(new StoneProvider());
        }
    }
}
