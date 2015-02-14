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
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetPreferences(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            return new JSResponse(request, this.CoreInstance.PlatformPreferenceDatabase.GetPreferences(platform));
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "preference", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "value", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse SetPreference(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            string preference = request.GetParameter("preference");
            string value = request.GetParameter("value");
            switch (preference)
            {
                case "emulator":
                    if (this.CoreInstance.PluginManager.LoadedEmulators.ContainsKey(value))
                    {
                        this.CoreInstance.PlatformPreferenceDatabase.SetEmulator(platform, value);
                        return new JSResponse(request, "Emulator set to " + value, true);
                    }
                    else
                    {
                        return new JSResponse(request, "Error: Emulator not installed", false);
                    }
                case "scraper":
                    if (this.CoreInstance.PluginManager.LoadedEmulators.ContainsKey(value))
                    {
                        this.CoreInstance.PlatformPreferenceDatabase.SetScraper(platform, value);
                        return new JSResponse(request, "Scraper set to " + value, true);
                    }
                    else
                    {
                        return new JSResponse(request, "Error: Scraper not installed", false);
                    }
                default:
                    return new JSResponse(request, "Error: unknown preference type", false);
            }
        }

        [AjaxMethod(MethodPrefix = "Platform")]
        public IJSResponse GetPlatforms(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.LoadedPlatforms);
        }
    }
}
