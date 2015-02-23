using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Reflection;
using Snowflake.Ajax;
using Snowflake.Service;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetEmulatorBridges(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.PluginManager.LoadedEmulators.ToDictionary
                (
                    emulatorBridge => emulatorBridge.Key,
                    emulatorBridge => emulatorBridge.Value.PluginInfo
                );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetEmulatorBridgesByPlatform(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.PluginManager.LoadedEmulators
                .Where(bridge => bridge.Value.SupportedPlatforms.Contains(request.GetParameter("platform"))).ToDictionary
                (
                    emulatorBridge => emulatorBridge.Key,
                    emulatorBridge => emulatorBridge.Value.PluginInfo
                );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetScrapers(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.PluginManager.LoadedScrapers.ToDictionary
               (
                   scraper => scraper.Key,
                   scraper => scraper.Value.PluginInfo
               );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetScrapersByPlatform(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.PluginManager.LoadedScrapers
                .Where(scraper => scraper.Value.SupportedPlatforms.Contains(request.GetParameter("platform"))).ToDictionary
               (
                   scraper => scraper.Key,
                   scraper => scraper.Value.PluginInfo
               );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllPlugins(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.PluginManager.Registry);

        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllAjaxMethods(IJSRequest request)
        {
            List<object> ajaxMethods = new List<object>();

            foreach (KeyValuePair<string, IBaseAjaxNamespace> ajaxNamespace in this.CoreInstance.AjaxManager.GlobalNamespace)
            {
                foreach (KeyValuePair<string, IJSMethod> jsMethod in ajaxNamespace.Value.JavascriptMethods)
                {
                    dynamic methodInfo = new ExpandoObject();
                    methodInfo.Namespace = ajaxNamespace.Key;
                    methodInfo.MethodName = jsMethod.Key;
                    methodInfo.Parameters = jsMethod.Value.MethodInfo.GetCustomAttributes<AjaxMethodParameterAttribute>()
                        .Select(attr => new Dictionary<string, object>() {
                        {"ParameterName", attr.ParameterName},
                        {"ParameterType", Enum.GetName(typeof(AjaxMethodParameterType), attr.ParameterType)},
                        {"Required", attr.Required}
                    });
                    ajaxMethods.Add(methodInfo);
                }
            }
            return new JSResponse(request, ajaxMethods);
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
