using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Snowflake.Ajax;

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
            var bridge = new JSBridge();
            bridge.RegisterNamespace("Core", new JSApiCore());
            var output = await bridge.CallMethod("Core", "Test", new JSRequest("", new Dictionary<string, string>()));
            Console.WriteLine(output);
            Console.ReadLine();
        }
    }
}
