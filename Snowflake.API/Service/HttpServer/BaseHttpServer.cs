using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.Threading;
namespace Snowflake.Service.HttpServer
{
    /// <inheritdoc/>
    public abstract class BaseHttpServer : IBaseHttpServer
    {
        HttpListener serverListener;
        Thread serverThread;
        bool cancel;

        public BaseHttpServer(int port)
        {
            serverListener = new HttpListener();
            serverListener.Prefixes.Add("http://localhost:" + port.ToString() + "/");
        }
        void IBaseHttpServer.StartServer()
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

        void IBaseHttpServer.StopServer()
        {
            this.cancel = true;
            this.serverListener.Abort();
            this.serverThread.Abort();
            this.serverListener = null;
            this.serverThread = null;
        }

        /// <summary>
        /// Implement this to handle the process loop
        /// </summary>
        /// <param name="context">The HTTP context of the server listener</param>
        /// <returns>A Task that is called asynchronously that outputs a stream of text to be written as the HttpResponse</returns>
        /// <see cref="Snowflake.Service.Server"/> for example implementations
        protected abstract Task Process(HttpListenerContext context);
    }
}
