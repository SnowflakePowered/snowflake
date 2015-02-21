using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Web;
using Snowflake.Ajax;
using Snowflake.Extensions;

namespace Snowflake.Service.HttpServer
{
    public class ApiServer : BaseHttpServer
    {
        public ApiServer():base(30001)
        {
            
        }

        protected override async Task Process(HttpListenerContext context)
        {
            var writer = new StreamWriter(context.Response.OutputStream);
            context.AddAccessControlHeaders();
            string getRequest = context.Request.Url.AbsolutePath.Remove(0,1); //Remove first slash
            string getUri = context.Request.Url.AbsoluteUri;
            int index = getUri.IndexOf("?", StringComparison.Ordinal);
            IDictionary<string, string> dictParams = new Dictionary<string, string>();

            if (getUri.Contains("?post"))
            {
                try
                {
                    using (var reader = new StreamReader(context.Request.InputStream)){
                        IDictionary<string, string> _dictParams = JsonConvert.DeserializeObject<IDictionary<string, string>>(reader.ReadToEnd());
                        if (!(_dictParams == null))
                        {
                            dictParams = _dictParams;
                        }
                    }
                }
                catch (JsonException)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(JSResponse.GetErrorResponse("missing methodname or namespace")));
                    writer.Flush();
                    context.Response.OutputStream.Close();
                }
            }
            else
            {
                if (index > 0)
                {
                    string rawParams = getUri.Substring(index).Remove(0, 1);
                    var nvcParams = HttpUtility.ParseQueryString(rawParams);
                    dictParams = nvcParams.AllKeys.ToDictionary(o => o, o => nvcParams[o]);
                }
            }
            
            if(!(getRequest.Split('/').Count() >= 2))
            {
                writer.WriteLine(JsonConvert.SerializeObject(JSResponse.GetErrorResponse("missing methodname or namespace")));
                writer.Flush();
                context.Response.OutputStream.Close();
            }
            var request = new JSRequest(getRequest.Split('/')[0], getRequest.Split('/')[1], dictParams);

            if (request.MethodParameters.ContainsKey("jsoncallback"))
            {
                context.Response.AppendHeader("Content-Type", "application/javascript");
            }
            else
            {
                context.Response.AppendHeader("Content-Type", "application/json");
            }
            
            writer.WriteLine(await ProcessRequest(request));
            writer.Flush();
            context.Response.OutputStream.Close();
        }

        private async Task<string> ProcessRequest(JSRequest args)
        {
            return await CoreService.LoadedCore.AjaxManager.CallMethodAsync(args);
        }
    }
}

