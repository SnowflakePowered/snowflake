using Snowflake.Loader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    public class StoneProviderComposer : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IStoneProvider>(new StoneProvider());
        }
    }
}
