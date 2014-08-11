using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Ajax.SnowflakeCore;
using Snowflake.Ajax;
using Snowflake.Core;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
           Run();
            Console.ReadLine();
        }

        public async static void Run()
        {
            var bridge = FrontendCore.LoadedCore.PluginManager.PluginR
          //  bridge.RegisterNamespace("Core", new SnowflakeJsApi());
            var output = await bridge.CallMethod(new JSRequest("Core","Test", new Dictionary<string, string>()));
            Console.WriteLine(output);
            Console.ReadLine();
        }
    }
}
