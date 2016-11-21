using System.Threading;
using System.Threading.Tasks;
using Mono.Net;

namespace Snowflake.Services.HttpServer
{
    public abstract class BaseHttpServer : IBaseHttpServer
    {
        HttpListener serverListener;
        Thread serverThread;
        bool cancel;

        /// <summary>
        /// The base httpserver
        /// </summary>
        /// <param name="port"></param>
        protected BaseHttpServer(int port)
        {
            this.serverListener = new HttpListener();
            this.serverListener.Prefixes.Add($"http://localhost:{port}/");
        }
        void IBaseHttpServer.StartServer()
        {
            this.serverThread = new Thread(
                () =>
                {
                    this.serverListener.Start();
                    while (!this.cancel)
                    {
                        HttpListenerContext context = this.serverListener.GetContext();
                        Task.Run(() => this.Process(context));
                    }
                }
                ) {IsBackground = true};
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
        /// <see cref="Snowflake.Services.Server"/> for example implementations
        protected abstract Task Process(HttpListenerContext context);
    }
}
