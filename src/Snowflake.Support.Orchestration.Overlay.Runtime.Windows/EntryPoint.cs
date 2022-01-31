using System.Runtime.InteropServices;
using System;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks;
using System.Reflection;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Hooks.Direct3D11;
using System.IO.Pipes;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using Snowflake.Orchestration.Ingame;

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
            Memory<byte> test = new byte[Marshal.SizeOf<GameWindowCommand>()];
            Console.WriteLine(npc.Read(test.Span));

            GameWindowCommand? command = GameWindowCommand.FromBuffer(test);

            if (command.HasValue)
            {
                var value = command.Value;
                Console.WriteLine(Enum.GetName(value.Type));
                if (command.Value.Type == GameWindowCommandType.Handshake)
                {
                    Console.WriteLine(value.HandshakeEvent.Guid.ToString("N"));
                }
            }
            else
            {
                Console.WriteLine("nope");
            }

            var ping = Guid.NewGuid();
            Console.WriteLine("ping " + ping.ToString("N"));
            npc.Write(new GameWindowCommand()
            {
                Magic = GameWindowCommand.GameWindowMagic,
                Type = GameWindowCommandType.Handshake,
                HandshakeEvent = new()
                {
                    Guid = ping
                }
            }.ToBuffer().Span);
            Console.WriteLine("read ok");
            Console.WriteLine(npc.Read(test.Span));
            GameWindowCommand? pong = GameWindowCommand.FromBuffer(test);
            if (pong.HasValue)
            {
                var value = pong.Value;
                Console.WriteLine(Enum.GetName(value.Type));
                if (value.Type == GameWindowCommandType.Handshake)
                {
                    Console.WriteLine(value.HandshakeEvent.Guid.ToString("N"));
                }
            } else
            {
                Console.WriteLine("what");
            }

            return 0;
        }
    }
}