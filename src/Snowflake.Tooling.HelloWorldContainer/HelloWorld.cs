using System;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Model.Game;
using Snowflake.Services;

namespace Snowflake.Tooling.HelloWorldContainer
{
    public class HelloWorld : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(ILogProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            serviceContainer.Get<ILogProvider>().GetLogger("HelloWorld").Info("Hello World!");
        }
    }
}
