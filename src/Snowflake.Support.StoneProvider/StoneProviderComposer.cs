using Snowflake.Loader;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Services
{
    public class StoneProviderComposer : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IServiceContainer serviceContainer)
        {
            serviceContainer.Get<IServiceRegistrationProvider>().RegisterService<IStoneProvider>(new StoneProvider());
        }
    }
}
