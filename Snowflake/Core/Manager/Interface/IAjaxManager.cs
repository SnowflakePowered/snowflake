using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Ajax;

namespace Snowflake.Service.Manager.Interface
{
    public interface IAjaxManager : ILoadableManager
    {
        IReadOnlyDictionary<string, IBaseAjaxNamespace> GlobalNamespace { get; }
        void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject);
        Task<string> CallMethodAsync(JSRequest request);
        string CallMethod(JSRequest request);
    }
}