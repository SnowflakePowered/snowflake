using System;
using System.Collections.Generic;
using Snowflake.Ajax;
using Snowflake.Emulator;
using Snowflake.Platform;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Platform")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse GetPreferences(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            return new JSResponse(request, this.CoreInstance.PlatformPreferenceDatabase.GetPreferences(platform));
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "preference", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        public IJSResponse SetPreference(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            string preference = request.GetParameter("preference");
            string value = request.GetParameter("value");
            switch (preference)
            {
                case "emulator":
                    if (this.CoreInstance.PluginManager.Plugins<IEmulatorBridge>().ContainsKey(value))
                    {
                        this.CoreInstance.PlatformPreferenceDatabase.SetEmulator(platform, value);
                        return new JSResponse(request, "Emulator set to {value}", true); //return emulator that was set
                    }
                    throw new ArgumentException("Emulator Not Found", new KeyNotFoundException());
                case "scraper":
                    if (this.CoreInstance.PluginManager.Plugins<IEmulatorBridge>().ContainsKey(value))
                    {
                        this.CoreInstance.PlatformPreferenceDatabase.SetScraper(platform, value);
                        return new JSResponse(request, $"Scraper set to {value}", true); //return scraper that was set
                    }
                    throw new ArgumentException("Scraper Not Found", new KeyNotFoundException());
                default:
                    throw new ArgumentException("Unknown preference type");
            }
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse GetPlatforms(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.LoadedPlatforms);
        }
    }
}
