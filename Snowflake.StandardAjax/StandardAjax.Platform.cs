using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Platform;
using Snowflake.Emulator.Input;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse GetPreferences(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse SetPreference(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse SetInputDevice(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse GetPlatforms(IJSRequest request)
        {
            return new JSResponse(request, null);
        }
    }
}
