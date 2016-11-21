using System;
using Fleck;

namespace Snowflake.Services.JSWebSocketServer
{
    /// <summary>
    /// When the socket has received a message
    /// </summary>
    public class SocketMessageReceivedEventArgs : EventArgs
    {
        public readonly string Message;
        public SocketMessageReceivedEventArgs(string message)
        {
            this.Message = message;
        }
    }
    /// <summary>
    /// When a socket has disconnected or opened a connection
    /// </summary>
    public class SocketConnectionEventArgs : EventArgs
    {
        public IWebSocketConnection Connection;
        public SocketConnectionEventArgs(IWebSocketConnection connection)
        {
            this.Connection = connection;
        }
    }
}
