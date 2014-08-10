using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Core.Interface;

namespace Snowflake.Core
{
    public class AjaxManager : IAjaxManager
    {
        public IDictionary<string, IBaseAjaxNamespace> JavascriptNamespace { get; private set; }
        public AjaxManager()
        {
            this.JavascriptNamespace = new Dictionary<string, IBaseAjaxNamespace>();
        }
        public void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject)
        {
            if (!this.JavascriptNamespace.ContainsKey(namespaceName))
                this.JavascriptNamespace.Add(namespaceName, namespaceObject);
        }
        
        public async Task<string> CallMethod(JSRequest request)
        {
            try
            {
                JSResponse result =
                    await
                        Task.Run(
                            () => this.JavascriptNamespace[request.NameSpace].JavascriptMethods[request.MethodName].Invoke(request));
                return result.GetJson();
            }
            catch (KeyNotFoundException)
            {
                return JsonConvert.Undefined;
            }
        }
    }
}
