using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.Reflection;
using Snowflake.Ajax;
using Snowflake.Service;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax : BaseAjaxNamespace
    {
        public StandardAjax([Import("coreInstance")] ICoreService coreInstance)
            : base(Assembly.GetExecutingAssembly(), coreInstance)
        {
            
        }
    }
}
