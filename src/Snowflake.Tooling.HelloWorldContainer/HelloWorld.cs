using System;
using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Platform;
using Snowflake.Services;

namespace Snowflake.Tooling.HelloWorldContainer
{
    public class HelloWorld : IComposable
    {
        /// <inheritdoc/>
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IModule composableModule, Loader.IServiceRepository serviceContainer)
        {
            foreach (IPlatformInfo platform in serviceContainer.Get<IStoneProvider>().Platforms.Values)
            {
                Console.WriteLine(platform.PlatformId);
            }
        }
    }
}
