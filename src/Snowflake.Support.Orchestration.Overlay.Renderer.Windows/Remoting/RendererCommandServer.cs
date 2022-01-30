using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting
{
    internal class RendererCommandServer
    {
        CancellationTokenSource TokenSource { get; }

        Thread Thread { get; set; }
        public RendererCommandServer()
        {
            this.TokenSource = new CancellationTokenSource();
        }

        public void Activate()
        {
            this.Thread = new Thread(ServerThread);
            this.Thread.Start(this.TokenSource.Token);
        }

        public void Stop()
        {
            this.TokenSource.Cancel();
        }

        public NamedPipeClientStream OpenNew()
        {
            return new NamedPipeClientStream("Snowflake.Orchestration.Renderer");
        }

        public void ServerThread(object? data)
        {
            Console.WriteLine("Started IPC server");
            var shutdownEvent = (CancellationToken)data;
            var pipeServer = new NamedPipeServerStream("Snowflake.Orchestration.Renderer", 
                PipeDirection.InOut, 1);

            pipeServer.WaitForConnection();
            Console.WriteLine("Connection Established");

            pipeServer.Write(RendererCommand.Handshake().ToBuffer());

            Console.WriteLine("Handshake established.");

            Span<byte> readBuffer = stackalloc byte[Marshal.SizeOf<RendererCommand>()];

            while (true)
            {
                if (shutdownEvent.IsCancellationRequested)
                    break;
                int next;
                if ((next = pipeServer.ReadByte()) == -1)
                    break;
                byte magic = (byte)next;
                if (magic != RendererCommand.RendererMagic)
                    continue;
                if ((next = pipeServer.ReadByte()) == -1)
                    break;

                RendererCommandType type = (RendererCommandType)next;

                RendererCommand command;
                unsafe
                {
                    readBuffer[0] = magic;
                    readBuffer[1] = (byte)type;
                    if (pipeServer.Read(readBuffer[2..]) != Marshal.SizeOf<RendererCommand>() - 2)
                        continue;
                    command = MemoryMarshal.Cast<byte, RendererCommand>(readBuffer)[0];
                }

                switch (command.Type)
                {
                    case RendererCommandType.Handshake:
                        break;
                    case RendererCommandType.SharedTextureHandle:
                        break;
                    case RendererCommandType.WndProcEvent:
                        break;
                    case RendererCommandType.MouseEvent:
                        break;
                    case RendererCommandType.CursorEvent:
                        break;
                    case RendererCommandType.ResizeEvent:
                        break;
                    case RendererCommandType.ShutdownEvent:
                        break;
                }
            }

            Console.WriteLine("Connection closed.");
        }
    }
}
