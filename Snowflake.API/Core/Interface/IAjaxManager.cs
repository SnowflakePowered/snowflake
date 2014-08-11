using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Ajax;

namespace Snowflake.Core.Interface
{
    public interface IAjaxManager
    {
        IDictionary<string, IBaseAjaxNamespace> JavascriptNamespace { get; }
        void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject);
        Task<string> CallMethodAsync(JSRequest request);
        string CallMethod(JSRequest request);
    }
}