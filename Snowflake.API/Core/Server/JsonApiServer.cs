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
namespace Snowflake.Core.Server
{
    public class APIServer:BaseHttpServer
    {
        public APIServer():base(30001)
        {
            
        }

        protected override async Task Process(HttpListenerContext context)
        {
            context.AddAccessControlHeaders();
            string getRequest = context.Request.Url.AbsolutePath.Remove(0,1); //Remove first slash
            string getUri = context.Request.Url.AbsoluteUri;
            int index = getUri.IndexOf("?");
            var dictParams = new Dictionary<string, string>();
            if ( index > 0 ){
                string rawParams = getUri.Substring(index).Remove (0, 1);
                var nvcParams = HttpUtility.ParseQueryString(rawParams);
                dictParams =  nvcParams.AllKeys.ToDictionary(o => o, o => nvcParams[o]);
            }
            var request = new JSRequest(getRequest.Split('/')[0], dictParams);
            var writer = new StreamWriter(context.Response.OutputStream);
            
            writer.WriteLine(await ProcessRequest(request));
            writer.Flush();
   
          
            context.Response.OutputStream.Close();
        }

        private async Task<string> ProcessRequest(JSRequest args)
        {
            string method = args.MethodName;
            var invokedMethod = typeof(AjaxManager).GetMethod(method);
            if (invokedMethod != null)
            {
                return  (string)invokedMethod.Invoke(this, new object[] { args });
            }
            else return "invalid";
        }
    }
}

