using Snowflake.Extensibility;
using Snowflake.Orchestration.Ingame;
using Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Browser;
using Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Renderer.Windows.Remoting
{
    internal class IngameCommandController
    {
        private ILogger Logger { get; }
        CancellationTokenSource TokenSource { get; }
        Thread WatchdogThread { get; set; }
        ConcurrentQueue<NamedPipeServerStream> OpenPipes { get; set; }

        public delegate void IngameCommandHandler(GameWindowCommand command);
        public event IngameCommandHandler CommandReceived;

        private Guid InstanceGuid { get; }
        public IngameCommandController(ILogger logger, Guid instanceGuid)
        {
            this.InstanceGuid = instanceGuid;
            this.Logger = logger;
            this.TokenSource = new();
            this.OpenPipes = new();
        }

        public void Start()
        {
            this.WatchdogThread = new Thread(async (data) => await ServerThread((CancellationToken)data));
            this.WatchdogThread.Start(this.TokenSource.Token);
        }

        public void Stop()
        {
            this.TokenSource.Cancel();
        }

        public NamedPipeClientStream OpenNew()
        {
            return new NamedPipeClientStream("Snowflake.Orchestration.Renderer-"+this.InstanceGuid.ToString("N"));
        }

        public void Broadcast(GameWindowCommand command)
        {
            Task.Run(async () => await this.BroadcastAsync(command)).ConfigureAwait(false);
        }

        public async Task BroadcastAsync(GameWindowCommand command)
        {
            List<NamedPipeServerStream> tempStreams = new();

            while (this.OpenPipes.TryDequeue(out var pipe))
            {
                if (!pipe.IsConnected)
                    await pipe.DisposeAsync(); // goodbye pipe
                await pipe.WriteAsync(command.ToBuffer(), this.TokenSource.Token);
                tempStreams.Add(pipe);
            }

            // add pipes back to working set.
            foreach (var pipe in tempStreams)
            {
                this.OpenPipes.Enqueue(pipe);
            }
        }

        public async Task ServerWorkThread(NamedPipeServerStream pipeServer, CancellationToken shutdownEvent)
        {
            // todo: handle broken pipe
            Console.WriteLine("in thead");
            Memory<byte> readBuffer = new byte[Marshal.SizeOf<GameWindowCommand>()];
            try
            {
                while (!shutdownEvent.IsCancellationRequested && pipeServer.IsConnected)
                {
                    Console.WriteLine("starting read...");
                    int bytesRead = await pipeServer.ReadAsync(readBuffer, shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested) 
                    {
                        this.Logger.Info("Cancellation requested.");
                        break;
                    }
                    
                    if (bytesRead == 0)
                    {
                        this.Logger.Info("Pipe closed");
                        break;
                    }
                    if (readBuffer.Span[0] != GameWindowCommand.GameWindowMagic)
                    {
                        this.Logger.Info("Unexpected magic number: " + readBuffer.Span[0]);
                        continue;
                    }

                    if (bytesRead != readBuffer.Length)
                    {
                        this.Logger.Info($"Unexpected length {bytesRead}, expected {readBuffer.Length}");
                        continue;
                    }

                    GameWindowCommand? commandBytes = GameWindowCommand.FromBuffer(readBuffer);
                    if (!commandBytes.HasValue)
                    {
                        this.Logger.Info($"Unexpected payload.");
                        continue;
                    }
                    var command = commandBytes.Value;

                    switch (command.Type)
                    {
                        case GameWindowCommandType.Handshake:
                            this.Logger.Info("Got handshake pong command in cmdthread " + this.InstanceGuid);
                            this.Logger.Info("Sending pong with " + command.HandshakeEvent.Guid.ToString("N"));
                            var buffer = command.ToBuffer();
                            await pipeServer.WriteAsync(buffer, shutdownEvent);
                            break;
                        case GameWindowCommandType.ShutdownEvent:
                            this.Logger.Info("cmdthread shutdown request for " + this.InstanceGuid);
                            this.TokenSource.Cancel();
                            break;
                        case GameWindowCommandType.WindowResizeEvent:
                        case GameWindowCommandType.WindowMessageEvent:
                        case GameWindowCommandType.OverlayTextureEvent:
                        case GameWindowCommandType.MouseEvent:
                        case GameWindowCommandType.CursorEvent:
                            this.CommandReceived?.Invoke(command);
                            break;
                    }
                }
            } 
            catch
            {
                this.Logger.Info("client pipe broken for " + this.InstanceGuid);
            }
            finally
            {
                pipeServer.Dispose();
                this.Logger.Info("client connection closed " + this.InstanceGuid);
            }
        }

        public async Task ServerThread(CancellationToken shutdownEvent)
        {
            this.Logger.Info("Started ingame overlay IPC server.");
            // todo proper cancellation.
            while(!shutdownEvent.IsCancellationRequested)
            {
                var pipeServer = new NamedPipeServerStream(
                    "Snowflake.Orchestration.Renderer-" + this.InstanceGuid.ToString("N"),
                    PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances,
                    PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                try
                {
                    await pipeServer.WaitForConnectionAsync(shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested)
                        break;
                    this.Logger.Info("Connection established to new client");
                    await pipeServer.WriteAsync(GameWindowCommand.Handshake(this.InstanceGuid).ToBuffer(), shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested)
                        break;
                    this.Logger.Info("Handshake sent to client, shunting to handler thread.");
                    this.OpenPipes.Enqueue(pipeServer);

                    // hand off ownership of pipe to handler thread.
                    new Thread(async (data) =>
                    {
                        (NamedPipeServerStream pipeServer, CancellationToken shutdownEvent) = 
                        ((NamedPipeServerStream, CancellationToken))data;
                        await this.ServerWorkThread(pipeServer, shutdownEvent);
                    }).Start((pipeServer, shutdownEvent));
                }

                catch(Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
            }
        }
    }
}
