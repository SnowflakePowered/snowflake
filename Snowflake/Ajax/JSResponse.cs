using System.Collections.Generic;
using Newtonsoft.Json;

namespace Snowflake.Ajax
{
    public class JSResponse : IJSResponse
    {
        public IJSRequest Request { get; }
        public dynamic Payload { get; }
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
            return new Dictionary<string, object>
            { 
                {"error", errorMessage},
                {"success", false}
            };
        }
        private static string ProcessJSONP(dynamic output, bool success, IJSRequest request)
        {
             string json = JsonConvert.SerializeObject(new Dictionary<string, object>
             {
                 {"request", request},
                 {"payload", output},
                 {"success", success},
                 {"type", "methodresponse"}
             });
            if (request.MethodParameters.ContainsKey("jsoncallback"))
            {
                json = request.MethodParameters["jsoncallback"] + $"({json});";
            }
            return json;
        }
    }
}
