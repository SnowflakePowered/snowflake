using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Tests.Composable;
using System;

namespace Snowflake.Tests.DummyComposable 
{
    public class DummyServiceComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IDummyComposable>(new DummyService());
        }
    }
}
