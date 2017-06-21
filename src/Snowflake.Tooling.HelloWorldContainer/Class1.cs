using Snowflake.Extensibility;
using System;

namespace Snowflake.Tooling.HelloWorldContainer
{
    public class HelloWorld : IPluginContainer
    {
        public void Compose(IServiceContainer serviceContainer)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
