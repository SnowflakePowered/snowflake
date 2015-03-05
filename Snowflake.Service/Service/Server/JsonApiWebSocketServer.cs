using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Service.HttpServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using Fleck;

namespace Snowflake.Service.JSWebSocketServer
{
    public class JsonApiWebSocketServer : IJSWebSocketServer
    {
        IWebSocketServer server;
        IList<IWebSocketConnection> connections;
        Thread serverThread;
        public event EventHandler<SocketConnectionEventArgs> SocketOpen;
        public event EventHandler<SocketConnectionEventArgs> SocketClose;
        public event EventHandler<SocketMessageReceivedEventArgs> SocketMessage;
        public JsonApiWebSocketServer(int port)
        {
            server = new WebSocketServer("ws://0.0.0.0:" + port.ToString());
            this.connections = new List<IWebSocketConnection>();
            this.SocketMessage += Process;
            this.SocketOpen += (s, e) => connections.Add(e.Connection);
            this.SocketClose += (s, e) => connections.Remove(e.Connection);
        }

        public void SendMessage(string message)
        {
            foreach (IWebSocketConnection connection in this.connections)
            {
                if (connection.IsAvailable)
                {
                    try
                    {
                        connection.Send(message);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine(String.Format("Unable to send message '{0}' to connection {1}", message, connection.ConnectionInfo.ClientIpAddress));
                    }
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
            catch (JsonException)
            {
                this.SendMessage(JsonConvert.SerializeObject(JSResponse.GetErrorResponse("invalid json")));
                return;
            }
            if (requestObject.ContainsKey("method") && requestObject.ContainsKey("namespace"))
            {
                methodName = (string)requestObject["method"];
                methodNamespace = (string)requestObject["namespace"];
            }
            else
            {
                this.SendMessage(JsonConvert.SerializeObject(JSResponse.GetErrorResponse("missing method or namespace")));
                return;
            }
            if (requestObject.ContainsKey("params"))
            {
                methodParams = requestObject["params"].ToObject<IDictionary<string, string>>();
            }
            var request = new JSRequest(methodNamespace, methodName, methodParams);
            this.SendMessage(await this.ProcessRequest(request));
        }
        
        private async Task<string> ProcessRequest(JSRequest args)
        {
            return await CoreService.LoadedCore.AjaxManager.CallMethodAsync(args);
        }

        void IBaseHttpServer.StartServer()
        {
            this.serverThread = new Thread(
                () =>
                    server.Start(socket =>
                     {
                         socket.OnOpen = () => this.OnSocketOpen(socket);
                         socket.OnClose = () => this.OnSocketClose(socket);
                         socket.OnMessage = message => this.OnMessage(message);
                     }));
            this.serverThread.IsBackground = true;
            this.serverThread.Start();
        }

        void IBaseHttpServer.StopServer()
        {
            this.serverThread.Abort();
        }
        private void OnSocketOpen(IWebSocketConnection connection)
        {
            if (this.SocketOpen != null)
            {
                this.SocketOpen(this, new SocketConnectionEventArgs(connection));
            }
        }
        private void OnSocketClose(IWebSocketConnection connection)
        {
            if (this.SocketClose != null)
            {
                this.SocketClose(this, new SocketConnectionEventArgs(connection));
            }
        }
        private void OnMessage(string message)
        {
            if (this.SocketMessage != null)
            {
                this.SocketMessage(this, new SocketMessageReceivedEventArgs(message));
            }
        }
    }
}
