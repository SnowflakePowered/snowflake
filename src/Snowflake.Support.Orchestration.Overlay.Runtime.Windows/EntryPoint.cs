using System.Runtime.InteropServices;
using System;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks;
using System.Reflection;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11;
using System.IO.Pipes;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;

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
            var npc = new NamedPipeClientStream("Snowflake.Orchestration.Renderer");
            npc.Connect();
            Span<byte> test = new byte[Marshal.SizeOf<RendererCommand>() + 1];
            Console.WriteLine( npc.Read(test));

            RendererCommand? command = RendererCommand.FromBuffer(test);

            if (command.HasValue)
            {
                Console.WriteLine(Enum.GetName(command.Value.Type));
            }
            else
            {
                Console.WriteLine("nope");
            }
            return 0;
        }
    }
}