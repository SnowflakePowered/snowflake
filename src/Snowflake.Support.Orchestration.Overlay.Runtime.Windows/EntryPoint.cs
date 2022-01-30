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
            var npc = new NamedPipeClientStream("Snowflake.Orchestration.Renderer-" + Guid.Empty.ToString("N"));
            npc.Connect();
            Memory<byte> test = new byte[Marshal.SizeOf<RendererCommand>()];
            Console.WriteLine(npc.Read(test.Span));

            RendererCommand? command = RendererCommand.FromBuffer(test);

            if (command.HasValue)
            {
                var value = command.Value;
                Console.WriteLine(Enum.GetName(value.Type));
                if (command.Value.Type == RendererCommandType.Handshake)
                {
                    Console.WriteLine(value.HandshakeParams.Guid.ToString("N"));
                }
            }
            else
            {
                Console.WriteLine("nope");
            }

            var ping = Guid.NewGuid();
            Console.WriteLine("ping " + ping.ToString("N"));
            npc.Write(new RendererCommand()
            {
                Magic = RendererCommand.RendererMagic,
                Type = RendererCommandType.Handshake,
                HandshakeParams = new()
                {
                    Guid = ping
                }
            }.ToBuffer().Span);
            Console.WriteLine("read ok");
            Console.WriteLine(npc.Read(test.Span));
            RendererCommand? pong = RendererCommand.FromBuffer(test);
            if (pong.HasValue)
            {
                var value = pong.Value;
                Console.WriteLine(Enum.GetName(value.Type));
                if (value.Type == RendererCommandType.Handshake)
                {
                    Console.WriteLine(value.HandshakeParams.Guid.ToString("N"));
                }
            } else
            {
                Console.WriteLine("what");
            }
            return 0;
        }
    }
}