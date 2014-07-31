using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
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

        public abstract void Process(HttpListenerContext context);
    }
}
