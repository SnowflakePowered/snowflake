using System.Reflection;
using Snowflake.Ajax;
using Snowflake.Extensibility;
using Snowflake.Service;

namespace Snowflake.StandardAjax
{
    [Plugin("StandardAjax")]
    public partial class StandardAjax : AjaxNamespace
    {
        public StandardAjax(ICoreService coreInstance)
            : base(coreInstance)
        {
            
        }
    }
}
