using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Ajax
{
    public class JSBridge
    {
        public IDictionary<string, JSApiCore> JavascriptNamespace;
        public JSBridge()
        {
            this.JavascriptNamespace = new Dictionary<string, JSApiCore>();
        }
        public void RegisterNamespace(string namespaceName, JSApiCore namespaceObject)
        {
            if (!this.JavascriptNamespace.ContainsKey(namespaceName))
                this.JavascriptNamespace.Add(namespaceName, namespaceObject);
        }
        
        public async Task<string> CallMethod(string namespaceName, string methodName, JSRequest request)
        {

            dynamic result = await Task.Run<dynamic>(
                () => 
                this.JavascriptNamespace[namespaceName].GetType().GetMethod(methodName).Invoke(this.JavascriptNamespace[namespaceName], new object[] {request})
            );

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
