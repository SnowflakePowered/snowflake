using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.Threading;
namespace Snowflake.Core.Server
{
    public abstract class BaseHttpServer
    {
        HttpListener serverListener;
        Thread serverThread;
        bool cancel;

        public BaseHttpServer(int port)
        {
            serverListener = new HttpListener();
            serverListener.Prefixes.Add("http://localhost:" + port.ToString() + "/");

        }
        public void StartServer()
        {
            this.serverThread = new Thread(
                () =>
                {
                    serverListener.Start();
                    while (!this.cancel)
                    {
                        HttpListenerContext context = serverListener.GetContext();
                        Task.Run(() => this.Process(context));
                    }
                }
            );
            this.serverThread.IsBackground = true;
            this.serverThread.Start();
        }

        public void StopServer()
        {
            this.cancel = true;
            this.serverThread.Join();
            this.serverListener.Stop();
        }

        protected abstract Task Process(HttpListenerContext context);

        public static void AddAccessControlHeaders(ref HttpListenerContext context)
        {
            context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
            context.Response.AppendHeader("Access-Control-Allow-Origin", "*");
            context.Response.AppendHeader("Access-Control-Origin", "*");
        }
    }
}
