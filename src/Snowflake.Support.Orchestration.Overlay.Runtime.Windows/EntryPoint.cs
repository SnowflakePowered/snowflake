using System.Runtime.InteropServices;
using System;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks;
using System.Reflection;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows
{

    public static class EntryPoint
    {
        public static int Main(nint args, int sizeBytes)
        {
            try
            {
                return Main();
            } 
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 1;
            }
        }

        private static int Main()
        {
            Vanara.PInvoke.Kernel32.AllocConsole();
            Console.WriteLine("Hello from C#! (" + RuntimeInformation.FrameworkDescription + ")");
            new Direct3D9Hook().Activate();
            return 0;
        }
    }
}