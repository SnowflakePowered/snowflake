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
namespace Snowflake.Service.Server
{
    public class ApiServer : BaseHttpServer
    {
        public ApiServer():base(30001)
        {
            
        }

        protected override async Task Process(HttpListenerContext context)
        {
            context.AddAccessControlHeaders();
            string getRequest = context.Request.Url.AbsolutePath.Remove(0,1); //Remove first slash
            string getUri = context.Request.Url.AbsoluteUri;
            int index = getUri.IndexOf("?", StringComparison.Ordinal);
            var dictParams = new Dictionary<string, string>();
            
            if (index > 0)
            {
                string rawParams = getUri.Substring(index).Remove(0, 1);
                var nvcParams = HttpUtility.ParseQueryString(rawParams);
                dictParams = nvcParams.AllKeys.ToDictionary(o => o, o => nvcParams[o]);
            }
            var request = getRequest.Split('/').Count() >= 2 ?
                new JSRequest(getRequest.Split('/')[0], getRequest.Split('/')[1], dictParams) :
                new JSRequest("", "", new Dictionary<string, string>());
            
            var writer = new StreamWriter(context.Response.OutputStream);
            
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

