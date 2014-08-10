using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Snowflake.Ajax
{
    public class AjaxManager
    {
        public IDictionary<string, BaseAjaxNamespace> JavascriptNamespace;
        public AjaxManager()
        {
            this.JavascriptNamespace = new Dictionary<string, BaseAjaxNamespace>();
        }
        public void RegisterNamespace(string namespaceName, BaseAjaxNamespace namespaceObject)
        {
            if (!this.JavascriptNamespace.ContainsKey(namespaceName))
                this.JavascriptNamespace.Add(namespaceName, namespaceObject);
        }
        
        public async Task<string> CallMethod(string namespaceName, string methodName, JSRequest request)
        {

            var result = await Task.Run(() => this.JavascriptNamespace[namespaceName].JavascriptMethods[methodName].Invoke(request));
            return result.GetJson();
        }
    }
}
