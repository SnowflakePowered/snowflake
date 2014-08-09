using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core.API;
using Newtonsoft.Json;
using Snowflake.Extensions;
using Snowflake.Information.Game;
using System.Dynamic;
namespace Snowflake.Core.JsonApi
{
    public class JSBridge
    {
        public IDictionary<string, IDictionary<string, Func<JSRequest, dynamic>>> JavascriptNamespace;
        public JSBridge()
        {
            this.JavascriptNamespace = new Dictionary<string, IDictionary<string, Func<JSRequest, dynamic>>>();
        }
        public void RegisterNamespace(string namespaceName)
        {
            if (!this.JavascriptNamespace.ContainsKey(namespaceName))
                this.JavascriptNamespace.Add(namespaceName, new Dictionary<string, Func<JSRequest, dynamic>>());
        }
        public void RegisterMethod(string namespaceName, string methodName, Func<JSRequest, dynamic> method)
        {
            this.JavascriptNamespace[namespaceName].Add(methodName, method);
        }
        public string CallMethod(string namespaceName, string methodName, JSRequest request)
        {
            dynamic result = Task.Run<dynamic>(this.JavascriptNamespace[namespaceName][methodName].Invoke(request));
            return ProcessJSONP(result, request);
        }
        private static string ProcessJSONP(dynamic output, JSRequest request){
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
