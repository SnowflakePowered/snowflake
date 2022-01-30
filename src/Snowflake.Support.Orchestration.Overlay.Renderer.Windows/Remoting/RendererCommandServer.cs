using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
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
        public Guid TabGuid { get; }
        public RendererCommandServer(Guid tabGuid)
        {
            this.TokenSource = new CancellationTokenSource();
            this.TabGuid = tabGuid;
        }

        public void Activate()
        {
            this.Thread = new Thread(async (data) => await ServerThread((CancellationToken)data));
            this.Thread.Start(this.TokenSource.Token);
        }

        public void Stop()
        {
            this.TokenSource.Cancel();
        }

        public NamedPipeClientStream OpenNew()
        {
            return new NamedPipeClientStream("Snowflake.Orchestration.Renderer-"+this.TabGuid.ToString("N"));
        }

        public async Task ServerWorkThread(NamedPipeServerStream pipeServer, CancellationToken shutdownEvent)
        {
            // todo: handle broken pipe

            Memory<byte> readBuffer = new byte[Marshal.SizeOf<RendererCommand>()];
            while (true)
            {
                if (shutdownEvent.IsCancellationRequested)
                    break;
                int bytesRead = await pipeServer.ReadAsync(readBuffer, shutdownEvent);
                if (shutdownEvent.IsCancellationRequested)
                    break;
                if (readBuffer.Span[0] != RendererCommand.RendererMagic || bytesRead != readBuffer.Length)
                    continue;
                

                RendererCommand? commandBytes = RendererCommand.FromBuffer(readBuffer);
                if (!commandBytes.HasValue)
                    continue;
                var command = commandBytes.Value;

                switch (command.Type)
                {
                    case RendererCommandType.Handshake:
                        Console.WriteLine("send pong handshake of " + command.HandshakeParams.Guid.ToString("N"));
                        var buffer = command.ToBuffer();
                        await pipeServer.WriteAsync(buffer, shutdownEvent);
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
            pipeServer.Dispose();
            Console.WriteLine("Connection closed.");
        }

        public async Task ServerThread(CancellationToken shutdownEvent)
        {
            Console.WriteLine("Started IPC server");
            // todo proper cancellation.
            while(!shutdownEvent.IsCancellationRequested)
            {
                var pipeServer = new NamedPipeServerStream(
                    "Snowflake.Orchestration.Renderer-" + this.TabGuid.ToString("N"),
                    PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances);
                try
                {
                    await pipeServer.WaitForConnectionAsync(shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested)
                        break;
                    Console.WriteLine("Connection Established");
                    await pipeServer.WriteAsync(RendererCommand.Handshake(this.TabGuid).ToBuffer(), shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested)
                        break;
                    Console.WriteLine("Handshake established.");
                    new Thread(async (data) =>
                    {
                        (NamedPipeServerStream pipeServer, CancellationToken shutdownEvent) = 
                        ((NamedPipeServerStream, CancellationToken))data;
                        await this.ServerWorkThread(pipeServer, shutdownEvent);
                    }).Start((pipeServer, shutdownEvent));
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
