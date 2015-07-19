using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Threading;
using Snowflake.Extensions;
using Snowflake.Service.HttpServer;
using Newtonsoft.Json;
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
            this.themeWebServer.Module<StaticFilesModule>().AddHandler(ModuleMap.AnyPath, HttpVerbs.Post, (server, context) => EchoThemes(context, server));
            this.themeWebServer.RunAsync();
        }
        private bool EchoThemes(HttpListenerContext context, WebServer server, bool sendBuffer = true)
        {
            HashSet<object> themesDirectory = new HashSet<object>();
            foreach (string directory in Directory.GetDirectories(this.ThemeRoot))
            {
                if (File.Exists(Path.Combine(directory, "theme.json"))) {
                    string themeInfo = File.ReadAllText(Path.Combine(directory, "theme.json"));
                    themesDirectory.Add(JsonConvert.DeserializeObject(themeInfo));
                }
            }
            byte[]output = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(themesDirectory));
            context.Response.OutputStream.Write(output, 0, output.Length);
            context.Response.OutputStream.Close();
            return true;
        }
        public void StopServer()
        {
            this.themeWebServer.Dispose();
        }
    }
}
