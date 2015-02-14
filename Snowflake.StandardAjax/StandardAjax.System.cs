using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Service;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetEmulatorBridges(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.PluginManager.LoadedEmulators);
        }

        [AjaxMethod(MethodPrefix = "System")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetEmulatorBridgesForPlatform(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.PluginManager.LoadedEmulators.Where(bridge => bridge.Value.SupportedPlatforms.Contains(request.GetParameter("platform"))));
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetScrapers(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetScrapersForPlatform(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllPlugins(IJSRequest request)
        {
            return new JSResponse(request, null);

        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllAjaxMethods(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse SendEmulatorPrompt(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse ShutdownCore(IJSRequest request)
        {
            return new JSResponse(request, null);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetCoreVersionString(IJSRequest request)
        {
            return new JSResponse(request, null);
        }
    }
}
