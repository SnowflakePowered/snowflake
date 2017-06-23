using Snowflake.Extensibility;
using Snowflake.Loader;
using Snowflake.Platform;
using Snowflake.Services;
using System;

namespace Snowflake.Tooling.HelloWorldContainer
{
    public class HelloWorld : IComposable
    {
        [ImportService(typeof(IStoneProvider))]
        public void Compose(IModule composableModule, IServiceContainer serviceContainer)
        {
            foreach (IPlatformInfo platform in serviceContainer.Get<IStoneProvider>().Platforms.Values)
            {
                Console.WriteLine(platform.PlatformID);
            }
        }
    }
}
