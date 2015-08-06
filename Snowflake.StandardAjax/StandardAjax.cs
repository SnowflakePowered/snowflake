using System.ComponentModel.Composition;
using System.Reflection;
using Snowflake.Ajax;
using Snowflake.Service;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax : BaseAjaxNamespace
    {
        [ImportingConstructor]
        public StandardAjax([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            
        }
    }
}
