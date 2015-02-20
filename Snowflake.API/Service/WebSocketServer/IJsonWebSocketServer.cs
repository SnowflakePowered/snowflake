using System;
namespace Snowflake.Service.Server
{
    /// <summary>
    /// Represents a web socket server for 2 way duplex communication
    /// </summary>
    public interface IJsonWebSocketServer
    {
        /// <summary>
        /// Send a message to all connected clients
        /// </summary>
        /// <param name="message">The message to send</param>
        void SendMessage(string message);
        /// <summary>
        /// When a socket connection has closed
        /// </summary>
        event EventHandler<SocketConnectionEventArgs> SocketClose;
        /// <summary>
        /// When the server has received a message
        /// </summary>
        event EventHandler<SocketMessageReceivedEventArgs> SocketMessage;
        /// <summary>
        /// When a new socket connection has opened
        /// </summary>
        event EventHandler<SocketConnectionEventArgs> SocketOpen;
        /// <summary>
        /// Start the WebSocketServer on a new thread
        /// </summary>
        void StartServer();
    }
}
