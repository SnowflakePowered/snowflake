using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;

namespace Ajax.SnowflakeCore
{
    public class SnowflakeJsApi : BaseAjaxNamespace
    {
        public SnowflakeJsApi() : base(Assembly.GetExecutingAssembly())
        {
            
        }

        [AjaxMethod]
        public JSResponse Test(JSRequest request)
        {
            return new JSResponse(request, "success from Api");
        }
    }
}
