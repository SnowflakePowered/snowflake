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
        public bool Success { get; set; }
        public JSResponse(IJSRequest request, dynamic payload, bool success = true)
        {
            this.Request = request;
            this.Payload = payload;
            this.Success = success;
        }

        public string GetJson()
        {
            return JSResponse.ProcessJSONP(this.Payload, this.Success, this.Request);
        }
        public static IDictionary<string, object> GetErrorResponse(string errorMessage)
        {
            return new Dictionary<string, object>() { 
                {"error", errorMessage},
                {"success", false},
            };
        }
        private static string ProcessJSONP(dynamic output, bool success, IJSRequest request)
        {
            if (request.MethodParameters.ContainsKey("jsoncallback"))
            {
                return request.MethodParameters["jsoncallback"] + "(" + JsonConvert.SerializeObject(new Dictionary<string, object>(){
                    {"request", request},
                    {"payload", output},
                    {"success", success},
                    {"type", "methodresponse"}
                }) + ");";
            }
            else
            {
                return JsonConvert.SerializeObject(new Dictionary<string, object>(){
                    {"request", request},
                    {"payload", output},
                    {"success", success},
                    {"type", "methodresponse"}
                });
            }
        }
    }
}
