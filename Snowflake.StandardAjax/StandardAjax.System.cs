using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Snowflake.Ajax;
using Snowflake.Emulator;
using Snowflake.Scraper;
using Snowflake.Service.Manager;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetEmulatorBridges(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.Get<IPluginManager>().Get<IEmulatorBridge>().ToDictionary
                (
                    emulatorBridge => emulatorBridge.Key,
                    emulatorBridge => emulatorBridge.Value.PluginProperties
                );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetEmulatorBridgesByPlatform(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.Get<IPluginManager>().Get<IEmulatorBridge>()
                .Where(bridge => bridge.Value.SupportedPlatforms.Contains(request.GetParameter("platform"))).ToDictionary
                (
                    emulatorBridge => emulatorBridge.Key,
                    emulatorBridge => emulatorBridge.Value.PluginProperties
                );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetScrapers(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.Get<IPluginManager>().Get<IScraper>().ToDictionary
               (
                   scrapers => scrapers.Key,
                   scrapers => scrapers.Value.PluginProperties
               );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetScrapersByPlatform(IJSRequest request)
        {
            IDictionary<string, IDictionary<string, dynamic>> response = this.CoreInstance.Get<IPluginManager>().Get<IScraper>()
                .Where(scraper => scraper.Value.SupportedPlatforms.Contains(request.GetParameter("platform"))).ToDictionary
               (
                   scraper => scraper.Key,
                   scraper => scraper.Value.PluginProperties
               );
            return new JSResponse(request, response);
        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllPlugins(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.Get<IPluginManager>().Registry);

        }

        [AjaxMethod(MethodPrefix = "System")]
        public IJSResponse GetAllAjaxMethods(IJSRequest request)
        {
            List<object> ajaxMethods = new List<object>();

            foreach (KeyValuePair<string, IAjaxNamespace> ajaxNamespace in this.CoreInstance.Get<IAjaxManager>().GlobalNamespace)
            {
                foreach (KeyValuePair<string, IJSMethod> jsMethod in ajaxNamespace.Value.JavascriptMethods)
                {
                    dynamic methodInfo = new ExpandoObject();
                    methodInfo.Namespace = ajaxNamespace.Key;
                    methodInfo.MethodName = jsMethod.Key;
                    methodInfo.Parameters = jsMethod.Value.MethodInfo.GetCustomAttributes<AjaxMethodParameterAttribute>()
                        .Select(attr => new Dictionary<string, object>
                        {
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
