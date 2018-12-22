using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Loader;
using Snowflake.Services;

namespace Snowflake.Framework.Sdk
{ 
    public class Composable : IComposable
    {
        [ImportService(typeof(IServiceRegistrationProvider))]
        public void Compose(IModule module, Loader.IServiceRepository serviceContainer)
        {
        }
    }
}
