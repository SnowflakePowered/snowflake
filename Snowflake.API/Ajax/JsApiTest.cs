using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Ajax
{
    public class JsApiTest:BaseAjaxNamespace
    {
        public JsApiTest() : base(Assembly.GetExecutingAssembly())
        {
            
        }

        [AjaxMethod]
        public JSResponse Test(JSRequest request)
        {
            return new JSResponse(request, "success");
        }
    }
}
