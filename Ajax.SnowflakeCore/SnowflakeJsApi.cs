using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Core;

namespace Ajax.SnowflakeCore
{
    public class SnowflakeJsApi : BaseAjaxNamespace
    {
        public SnowflakeJsApi() : base(Assembly.GetExecutingAssembly())
        {
            
        }

        [AjaxMethod(MethodPrefix = "Get", MethodName = "NotTest")]
        public JSResponse Test(JSRequest request)
        {
            return new JSResponse(request, "success from Api");
        }

        [AjaxMethod]
        public JSResponse GetTest(JSRequest request)
        {
            var game = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID("sWJznptYf0m_qH0_OvHtSg");
            return  new JSResponse(request, game);
        }
    }
}
