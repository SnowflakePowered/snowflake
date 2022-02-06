using Snowflake.Orchestration.Ingame;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows
{
    internal class IngameIpc
    {
        public IngameIpc(Guid instanceGuid)
        {
            this.Cancellation = new CancellationTokenSource();
            this.InstanceGuid = instanceGuid;
            this.ClientStream = new NamedPipeClientStream(".", "Snowflake.Orchestration.Renderer-" + this.InstanceGuid.ToString("N"), PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        private CancellationTokenSource Cancellation { get; }
        private Guid InstanceGuid { get; }
        private NamedPipeClientStream ClientStream { get; }

        public delegate void IngameCommandHandler(GameWindowCommand command);
        public event IngameCommandHandler CommandReceived;

        private bool IsListening { get; set; } = false;
        public async ValueTask<IpcConnectStatus> ConnectAsync(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("Waiting for IPC connection...");
            Memory<byte> handshakePacket = new byte[Marshal.SizeOf<GameWindowCommand>()];
            try
            {
                await this.ClientStream.ConnectAsync(cancellationToken);
                this.SendRequest(GameWindowCommand.Handshake(this.InstanceGuid));
                await this.ClientStream.ReadAsync(handshakePacket, cancellationToken);
            } 
            catch
            {
                return IpcConnectStatus.ConnectionError;
            }
            if (cancellationToken.IsCancellationRequested)
                return IpcConnectStatus.ConnectionTimeout;

            // failed to parse packet
            GameWindowCommand? _command = GameWindowCommand.FromBuffer(handshakePacket);
            if (!_command.HasValue)
                return IpcConnectStatus.InvalidCommand;

            // got unexpected handshake??
            GameWindowCommand handshake = _command.Value;
            if (handshake.Type != GameWindowCommandType.Handshake
                && handshake.HandshakeEvent.Guid != this.InstanceGuid)
                return IpcConnectStatus.InvalidHandshake;

            return IpcConnectStatus.Success;
        }

        public bool SendRequest(GameWindowCommand command)
        {
            // todo: reconnect on failure. Will need async :(
            Span<byte> buffer = stackalloc byte[Marshal.SizeOf<GameWindowCommand>()];
            command.IntoBuffer(ref buffer);

            // this should return immediately...
            try
            {
                this.ClientStream.Write(buffer);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Listen()
        {
            if (this.IsListening)
                return;
            Task.Run(async () => await this.ServerWorkThread(this.Cancellation.Token));
            this.IsListening = true;
        }

        public async ValueTask ServerWorkThread(CancellationToken shutdownEvent)
        {
            Memory<byte> readBuffer = new byte[Marshal.SizeOf<GameWindowCommand>()];
            Console.WriteLine("IPC Thread Running...");
            try
            {
                while (!shutdownEvent.IsCancellationRequested && this.ClientStream.IsConnected)
                {
                    int bytesRead = await this.ClientStream.ReadAsync(readBuffer, shutdownEvent);
                    if (shutdownEvent.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation requested.");
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Pipe closed");
                        break;
                    }
                    if (readBuffer.Span[0] != GameWindowCommand.GameWindowMagic)
                    {
                        Console.WriteLine("Unexpected magic number: " + readBuffer.Span[0]);
                        continue;
                    }

                    if (bytesRead != readBuffer.Length)
                    {
                        Console.WriteLine($"Unexpected length {bytesRead}, expected {readBuffer.Length}");
                        continue;
                    }

                    GameWindowCommand? commandBytes = GameWindowCommand.FromBuffer(readBuffer);
                    if (!commandBytes.HasValue)
                    {
                        Console.WriteLine($"Unexpected payload.");
                        continue;
                    }
                    var command = commandBytes.Value;

                    switch (command.Type)
                    {
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
            }
            finally
            {
                this.ClientStream.Dispose();
            }
        }

        public void Stop()
        {
            this.Cancellation.Cancel();
        }

        public enum IpcConnectStatus
        {
            Success,
            InvalidHandshake,
            InvalidCommand,
            ConnectionTimeout,
            ConnectionError
        }
    }
}
