using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Ajax
{
    public class JSResponse : IJSResponse
    {
        public IJSRequest Request { get; private set; }
        public dynamic Payload { get; private set; }
        public bool Result { get; set; }
        public JSResponse(IJSRequest request, dynamic payload, bool result = true)
        {
            this.Request = request;
            this.Payload = payload;
            this.Result = result;
        }

        public string GetJson()
        {
            return JSResponse.ProcessJSONP(this.Payload, this.Result, this.Request);
        }
        private static string ProcessJSONP(dynamic output, bool result, IJSRequest request)
        {
            if (request.MethodParameters.ContainsKey("jsoncallback"))
            {
                return request.MethodParameters["jsoncallback"] + "(" + JsonConvert.SerializeObject(new Dictionary<string, object>(){
                    {"payload", output},
                    {"result", result}
                }) + ");";
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>(){
                    {"payload", output},
                    {"result", result}
                });
            }
        }
    }
}
