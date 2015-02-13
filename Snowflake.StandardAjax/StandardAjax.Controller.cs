using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Controller;
using Snowflake.Emulator.Input;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetProfiles(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse SetInput(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse LoadFileProfile(IJSRequest request)
        {
            return new JSResponse(request, null);
        }
    }
}
