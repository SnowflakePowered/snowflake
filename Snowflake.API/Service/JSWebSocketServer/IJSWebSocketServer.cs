using System;
using Snowflake.Service.HttpServer;
namespace Snowflake.Service.JSWebSocketServer
{
    /// <summary>
    /// Represents a web socket server for 2 way duplex communication
    /// </summary>
    public interface IJSWebSocketServer : IBaseHttpServer
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
    }
}
