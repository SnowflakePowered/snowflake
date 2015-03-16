using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Controller;
using Snowflake.Platform;
using Snowflake.Emulator.Input;
using Newtonsoft.Json;
namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "controller", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetProfiles(IJSRequest request)
        {
            string controllerId = request.GetParameter("controller");
            IControllerProfileStore profileStore = this.CoreInstance.LoadedControllers[controllerId].ProfileStore;
            IDictionary<string, object> controllerProfiles = new Dictionary<string, object>();
            foreach (string deviceName in profileStore.AvailableProfiles)
            {
                controllerProfiles.Add(deviceName, profileStore.GetControllerProfile(deviceName));
            }
            return new JSResponse(request, controllerProfiles);
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "controller", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.StringParameter)]

        public IJSResponse GetProfileForDevice(IJSRequest request)
        {
            string controllerId = request.GetParameter("controller");
            string deviceName = request.GetParameter("device");
            IControllerProfile profile = this.CoreInstance.LoadedControllers[controllerId].ProfileStore[deviceName];
            return new JSResponse(request, profile);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "inputconfig", ParameterType = AjaxMethodParameterType.ObjectParameter)]
        [AjaxMethodParameter(ParameterName = "controller", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.StringParameter)]

        public IJSResponse SetInputConfiguration(IJSRequest request)
        {
            string controllerId = request.GetParameter("controller");
            string deviceName = request.GetParameter("device");
            IDictionary<string, string> changes = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.GetParameter("inputconfig"));
            IControllerProfile profile = this.CoreInstance.LoadedControllers[controllerId].ProfileStore[deviceName];
            foreach (KeyValuePair<string, string> change in changes) 
            {
                profile.InputConfiguration[change.Key] = change.Value;
            }
            this.CoreInstance.LoadedControllers[controllerId].ProfileStore.SetControllerProfile(deviceName, profile);
            return new JSResponse(request, "success");
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetControllers(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.LoadedControllers);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        [AjaxMethodParameter(ParameterName = "port", ParameterType = AjaxMethodParameterType.IntParameter)]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.IntParameter)]
        public IJSResponse SetDeviceInPort(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            string deviceName = request.GetParameter("device");
            int port = Int32.Parse(request.GetParameter("port"));
            this.CoreInstance.ControllerPortsDatabase.SetDeviceInPort(platform, port, deviceName);

            return new JSResponse(request, null);
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetInputDevices(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.InputManager.GetGamepads());
        }
    }
}
