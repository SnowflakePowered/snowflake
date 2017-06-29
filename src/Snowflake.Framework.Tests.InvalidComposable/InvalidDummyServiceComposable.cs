using Snowflake.Loader;
using Snowflake.Services;
using Snowflake.Tests.Composable;
using System;

namespace Snowflake.Tests.InvalidComposable
{
    public class InvalidDummyServiceComposable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule composableModule, Loader.IServiceProvider serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IInvalidService>(new InvalidService());
        }
    }
}
