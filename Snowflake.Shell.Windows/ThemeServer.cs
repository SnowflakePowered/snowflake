﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Services.HttpServer;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;

namespace Snowflake.Shell.Windows
{
    public class ThemeServer : IBaseHttpServer
    {
        public string ThemeRoot { get; set; }
        private readonly WebServer themeWebServer;
        public ThemeServer(string themeRoot) 
        {
            this.ThemeRoot = themeRoot;
            if (!Directory.Exists(this.ThemeRoot))
            {
                Directory.CreateDirectory(this.ThemeRoot);
            }
            var url = "http://localhost:30000/";
            this.themeWebServer = new WebServer(url);
        }

        public void StartServer()
        {

            this.themeWebServer.RegisterModule(new StaticFilesModule(this.ThemeRoot));
            this.themeWebServer.Module<StaticFilesModule>().UseRamCache = true;
            this.themeWebServer.Module<StaticFilesModule>().DefaultExtension = ".html";
            this.themeWebServer.Module<StaticFilesModule>().DefaultDocument = "index.html";
            this.themeWebServer.Module<StaticFilesModule>().AddHandler("/themedata/", HttpVerbs.Post, (server, context) => this.EchoThemes(context, server));
            this.themeWebServer.RunAsync();
        }
        private bool EchoThemes(HttpListenerContext context, WebServer server, bool sendBuffer = true)
        {
            HashSet<object> themesDirectory = new HashSet<object>();
            foreach (string themeInfo in from directory in Directory.GetDirectories(this.ThemeRoot)
                                         where File.Exists(Path.Combine(directory, "theme.json"))
                                         select File.ReadAllText(Path.Combine(directory, "theme.json")))
            {
                themesDirectory.Add(JsonConvert.DeserializeObject(themeInfo));
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
