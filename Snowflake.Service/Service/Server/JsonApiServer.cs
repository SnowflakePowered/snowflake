using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Mono.Net;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Extensions;
using Snowflake.Services.Manager;

namespace Snowflake.Services.HttpServer
{
    public class ApiServer : BaseHttpServer
    {
        private ICoreService coreInstance;

        public ApiServer(ICoreService coreInstance):base(30001)
        {
            this.coreInstance = coreInstance;
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
                        dictParams = _dictParams ?? dictParams;
                    }
                }
                catch (JsonException ex)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(JSResponse.GetErrorResponse(new JSException(ex))));
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

                writer.WriteLine(JsonConvert.SerializeObject(JSResponse.GetErrorResponse(new JSException(new KeyNotFoundException("Method or Namespace key not found in request JSON")))));
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
            
            writer.WriteLine(await this.ProcessRequest(request));
            writer.Flush();
            context.Response.OutputStream.Close();
        }

        private async Task<string> ProcessRequest(JSRequest args)
        {
            return await this.coreInstance.Get<IAjaxManager>().CallMethodAsync(args);
        }
    }
}

