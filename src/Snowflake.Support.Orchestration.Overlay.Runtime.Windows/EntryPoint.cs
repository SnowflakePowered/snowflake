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
using Silk.NET.Vulkan;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows
{

    public static class EntryPoint
    {
        public static int Main(nint args, int sizeBytes)
        {
            Vanara.PInvoke.Kernel32.AllocConsole();

            try
            {
                unsafe
                {
                    nint* handles = (nint*)args;
                    if (handles != null) 
                    {
                        Console.WriteLine($"ip: {handles[0]}, dp: {handles[1]},  {sizeBytes}");
                        return Main(new Instance(handles[0]), new Device(handles[1]));
                    }
                }
                return Main(null, null);
            } 
            catch(Exception e)
            {
                Console.WriteLine($"failed:\n{e}");
                //Console.ReadLine();
                return 1;
            }
        }

        private static int Main(Instance? i, Device? d)
        {
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
                new VulkanHook(i, d, ipc).Activate();

                new Direct3D11Hook(ipc).Activate();
                new OpenGLHook(ipc).Activate();

            });
            
            return 0;
        }
    }
}