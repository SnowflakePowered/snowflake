using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.IO;
using System.Threading;
namespace Snowflake.Core.Theme
{
    public class ThemeServer
    {
        HttpListener serverListener;
        Thread serverThread;
        public string ThemeRoot { get; set; }
        public ThemeServer(string themeRoot)
        {
            this.ThemeRoot = themeRoot;
            serverListener = new HttpListener();
            serverListener.Prefixes.Add("http://localhost:8999/");
        }
        public void StartServer()
        {
            this.serverThread = new Thread(
                () =>
                {
                    serverListener.Start();
                    while (true)
                    {
                        HttpListenerContext context = serverListener.GetContext();
                        this.Process(context);
                    }
                }
            );
                       
            this.serverThread.Start();
            
            
            
        }
        private void Process(HttpListenerContext context)
        {
            string filename = context.Request.Url.AbsolutePath;
            Console.WriteLine(filename);
            filename = filename.Substring(1);
            if (string.IsNullOrEmpty(filename))
                filename = "index.html";
            filename = Path.Combine(this.ThemeRoot, filename);
            Stream input;
            try
            {
                input = new FileStream(filename, FileMode.Open);
            }
            catch (FileNotFoundException)
            {
                input = new FileStream("index.html", FileMode.Open);
            }
            byte[] buffer = new byte[1024 * 16];
            int nbytes;
            while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                context.Response.OutputStream.Write(buffer, 0, nbytes);
            input.Close();
            context.Response.OutputStream.Close();
        }

        public void StopServer()
        {
            this.serverThread.Abort();
            this.serverListener.Stop();
        }
    }
}
