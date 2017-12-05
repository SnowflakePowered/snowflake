using System;
using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Tests.Composable;

namespace Snowflake.Tests.InvalidComposable
{
    public class InvalidDummyServiceComposable : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IInvalidService>(new InvalidService());
        }
    }
}
