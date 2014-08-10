using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Ajax
{
    public class JSResponse
    {
        public JSRequest Request { get; private set; }
        public dynamic Payload { get; private set; }
        
        public JSResponse(JSRequest request, dynamic payload)
        {
            this.Request = request;
            this.Payload = payload;
        }

        public string GetJson()
        {
            return JSResponse.ProcessJSONP(this.Payload, this.Request);
        }
        private static string ProcessJSONP(dynamic output, JSRequest request)
        {
            if (request.MethodParameters.ContainsKey("jsoncallback"))
            {
                return request.MethodParameters["jsoncallback"] + "(" + JsonConvert.SerializeObject(output) + ");";
            }
            else
            {
                return JsonConvert.SerializeObject(output);
            }
        }
        


    }
}
