using System.Runtime.InteropServices;
using System;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks;
using System.Reflection;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11;
using System.IO.Pipes;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using Snowflake.Orchestration.Ingame;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.OpenGL;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Vulkan;

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
            catch
            {
                Console.ReadLine();
                return 1;
            }
        }

        private static int Main()
        {
            Vanara.PInvoke.Kernel32.AllocConsole();
            Console.WriteLine("Hello from C#! (" + RuntimeInformation.FrameworkDescription + ")");
            var ipc = new IngameIpc(Guid.Empty);

            ipc.CommandReceived += (c) =>
            {
                Console.WriteLine($"Received command {c.Type}");
            };
            Task.Run(async () =>
            {
                var result = await ipc.ConnectAsync();
                Console.WriteLine($"IPC Connection: {result}");
                ipc.Listen();
                //new VulkanHook(ipc).Activate();

                //new Direct3D11Hook(ipc).Activate();
                new OpenGLHook(ipc).Activate();

            });
            
            return 0;
        }
    }
}