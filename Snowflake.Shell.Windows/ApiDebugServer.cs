using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Service.HttpServer;
using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Log;
using Unosquare.Labs.EmbedIO.Modules;

namespace Snowflake.Shell.Windows
{
    public class ApiDebugServer : IBaseHttpServer
    {
        public string ThemeRoot { get; set; }
        private readonly WebServer themeWebServer;
        public ApiDebugServer(string themeRoot) 
        {
            this.ThemeRoot = themeRoot;
          
            var url = "http://localhost:30001/";
            this.themeWebServer = new WebServer("http://localhost:9696/", new NullLog(), RoutingStrategy.Regex);
        }

        public void StartServer()
        {

            this.themeWebServer.RegisterModule(new WebApiModule());
            this.themeWebServer.Module<WebApiModule>().RegisterController<Controller>();
            this.themeWebServer.RunAsync();
        }

        public void StopServer()
        {
            this.themeWebServer.Dispose();
        }
    }

    class Controller : WebApiController
    {
        [WebApiHandler(HttpVerbs.Get, "/api/{id}")]
        public bool GetPeople(WebServer server, HttpListenerContext context, int id)
        {
            return context.JsonResponse("Hello World" + id);
        }

        protected bool HandleError(HttpListenerContext context, Exception ex, int statusCode = 500)
        {
            var errorResponse = new
            {
                Title = "Unexpected Error",
                ErrorCode = ex.GetType().Name,
                Description = ex.ExceptionMessage(),
            };

            context.Response.StatusCode = statusCode;
            return context.JsonResponse(errorResponse);
        }
    }
}
