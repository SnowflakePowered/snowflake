using System;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Tests.Composable;

namespace Snowflake.Tests.DummyComposable
{
    public class DummyServiceComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IDummyComposable>(new DummyService());
        }
    }
}
