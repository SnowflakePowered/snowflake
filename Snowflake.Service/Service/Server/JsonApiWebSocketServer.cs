﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Fleck;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Services.HttpServer;
using Snowflake.Services.Manager;

namespace Snowflake.Services.JSWebSocketServer
{
    public class JsonApiWebSocketServer : IJSWebSocketServer
    {
        IWebSocketServer server;
        private ICoreService coreInstance;
        readonly IList<IWebSocketConnection> connections;
        Thread serverThread;
        public event EventHandler<SocketConnectionEventArgs> SocketOpen;
        public event EventHandler<SocketConnectionEventArgs> SocketClose;
        public event EventHandler<SocketMessageReceivedEventArgs> SocketMessage;
        public JsonApiWebSocketServer(int port, ICoreService coreInstance)
        {
            this.coreInstance = coreInstance;
            this.server = new WebSocketServer($"ws://0.0.0.0:{port}");
            this.connections = new List<IWebSocketConnection>();
            this.SocketMessage += this.Process;
            this.SocketOpen += (s, e) => this.connections.Add(e.Connection);
            this.SocketClose += (s, e) => this.connections.Remove(e.Connection);
        }

        public void SendMessage(string message)
        {
            foreach (IWebSocketConnection connection in this.connections.Where(connection => connection.IsAvailable))
            {
                try
                {
                    connection.Send(message);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Unable to send message '{message}' to connection {connection.ConnectionInfo.ClientIpAddress}");
                }
            }
        }

        private async void Process(object sender, SocketMessageReceivedEventArgs e)
        {
            string methodName;
            string methodNamespace;
            IDictionary<string, string> methodParams = new Dictionary<string, string>();
            IDictionary<string, dynamic> requestObject = null;
            try
            {
                requestObject = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(e.Message);
            }
            catch (JsonException exeception)
            {
                this.SendMessage(JsonConvert.SerializeObject(JSResponse.GetErrorResponse(new JSException(exeception))));
                return;
            }
            if (requestObject.ContainsKey("method") && requestObject.ContainsKey("namespace"))
            {
                methodName = (string)requestObject["method"];
                methodNamespace = (string)requestObject["namespace"];
            }
            else
            {
                this.SendMessage(JsonConvert.SerializeObject(JSResponse.GetErrorResponse(new JSException(new KeyNotFoundException("Method or Namespace key not found in request JSON")))));
                return;
            }
            if (requestObject.ContainsKey("params"))
            {
                methodParams = requestObject["params"].ToObject<IDictionary<string, string>>();
            }
            var request = new JSRequest(methodNamespace, methodName, methodParams);
            this.SendMessage(await this.ProcessRequest(request));
        }
        
        private async Task<string> ProcessRequest(IJSRequest args)
        {
            return await this.coreInstance.Get<IAjaxManager>().CallMethodAsync(args);
        }

        void IBaseHttpServer.StartServer()
        {
            this.serverThread = new Thread(
                () => this.server.Start(socket =>
                {
                    socket.OnOpen = () => this.OnSocketOpen(socket);
                    socket.OnClose = () => this.OnSocketClose(socket);
                    socket.OnMessage = this.OnMessage;
                })) {IsBackground = true};
            this.serverThread.Start();
        }

        void IBaseHttpServer.StopServer()
        {
            this.server.Dispose();
            this.serverThread.Abort();
            this.server = null;
            this.serverThread = null;
        }
        private void OnSocketOpen(IWebSocketConnection connection)
        {
            this.SocketOpen?.Invoke(this, new SocketConnectionEventArgs(connection));
        }
        private void OnSocketClose(IWebSocketConnection connection)
        {
            this.SocketClose?.Invoke(this, new SocketConnectionEventArgs(connection));
        }
        private void OnMessage(string message)
        {

           this.SocketMessage?.Invoke(this, new SocketMessageReceivedEventArgs(message));
        }
    }
}
