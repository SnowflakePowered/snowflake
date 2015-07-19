using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Snowflake.Extensions;
using Snowflake.Service.HttpServer;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Log;
using Unosquare.Labs.EmbedIO.Modules;
namespace Snowflake.Shell.Windows
{
    public class ThemeServer : IBaseHttpServer
    {
        public string ThemeRoot { get; set; }
        private WebServer themeWebServer;
        public ThemeServer(string themeRoot) 
        {
            this.ThemeRoot = themeRoot;
            var url = "http://localhost:30000/";
            this.themeWebServer = new WebServer(url);
        }

        public void StartServer()
        {

            this.themeWebServer.RegisterModule(new StaticFilesModule(this.ThemeRoot));
            this.themeWebServer.Module<StaticFilesModule>().UseRamCache = true;
            this.themeWebServer.Module<StaticFilesModule>().DefaultExtension = ".html";
            this.themeWebServer.Module<StaticFilesModule>().DefaultDocument = "index.html";
            this.themeWebServer.RunAsync();
        }
        public void StopServer()
        {
            this.themeWebServer.Dispose();
        }
    }
}
