using System.Runtime.InteropServices;
using System;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks;
using System.Reflection;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11;

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
            new Direct3D11Hook().Activate();
            return 0;
        }
    }
}