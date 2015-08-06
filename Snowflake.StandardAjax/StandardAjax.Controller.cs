using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Snowflake.Ajax;
using Snowflake.Controller;
using Snowflake.Platform;

namespace Snowflake.StandardAjax
{
    public partial class StandardAjax
    {
        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetGamepadAbstractions(IJSRequest request)
        {
            IList<IGamepadAbstraction> gamepadAbstractions = this.CoreInstance.GamepadAbstractionDatabase.GetAllGamepadAbstractions();
            return new JSResponse(request, gamepadAbstractions);
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.StringParameter, Required=true)]

        public IJSResponse GetAbstractionForDevice(IJSRequest request)
        {
            string deviceName = request.GetParameter("device");
            IGamepadAbstraction gamepadAbstraction = this.CoreInstance.GamepadAbstractionDatabase[deviceName];
            return new JSResponse(request, gamepadAbstraction);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "inputconfig", ParameterType = AjaxMethodParameterType.ObjectParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "profiletype", ParameterType = AjaxMethodParameterType.StringParameter, Required = false)]

        public IJSResponse SetGamepadAbstraction(IJSRequest request)
        {
            string deviceName = request.GetParameter("device");
            string _profileType = request.GetParameter("profiletype");
            IGamepadAbstraction gamepadAbstraction;
            try
            {
                gamepadAbstraction = this.CoreInstance.GamepadAbstractionDatabase[deviceName];
            }
            catch
            {
                ControllerProfileType profileType;
                if(Enum.TryParse(_profileType, true, out profileType)){
                    gamepadAbstraction = new GamepadAbstraction(deviceName, profileType);
                }else{
                    gamepadAbstraction = new GamepadAbstraction(deviceName, ControllerProfileType.GAMEPAD_PROFILE);
                }
            }
            IDictionary<string, string> changes = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.GetParameter("inputconfig"));
            foreach (KeyValuePair<string, string> change in changes) 
            {
                gamepadAbstraction[change.Key] = change.Value;
            }
            this.CoreInstance.GamepadAbstractionDatabase[deviceName] = gamepadAbstraction;
            return new JSResponse(request, "success");
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetControllers(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.LoadedControllers);
        }

        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "port", ParameterType = AjaxMethodParameterType.IntParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "device", ParameterType = AjaxMethodParameterType.IntParameter, Required = true)]
        public IJSResponse SetDeviceInPort(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            string deviceName = request.GetParameter("device");
            int port = int.Parse(request.GetParameter("port"));
            this.CoreInstance.ControllerPortsDatabase.SetDeviceInPort(platform, port, deviceName);

            return new JSResponse(request, "success");
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter, Required = true)]
        [AjaxMethodParameter(ParameterName = "port", ParameterType = AjaxMethodParameterType.IntParameter, Required = true)]
        public IJSResponse GetDeviceInPort(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            int port = int.Parse(request.GetParameter("port"));

            return new JSResponse(request, this.CoreInstance.ControllerPortsDatabase.GetDeviceInPort(platform, port));
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        [AjaxMethodParameter(ParameterName = "platform", ParameterType = AjaxMethodParameterType.StringParameter)]
        public IJSResponse GetDeviceInPorts(IJSRequest request)
        {
            IPlatformInfo platform = this.CoreInstance.LoadedPlatforms[request.GetParameter("platform")];
            string[] ports = new string[9];
            for(int i = 1; i <= 8; i++){
                ports[i] = this.CoreInstance.ControllerPortsDatabase.GetDeviceInPort(platform, i);
            }
            return new JSResponse(request, ports);
        }
        [AjaxMethod(MethodPrefix = "Controller")]
        public IJSResponse GetInputDevices(IJSRequest request)
        {
            return new JSResponse(request, this.CoreInstance.InputManager.GetGamepads());
        }
    }
}
