using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input;
using Snowflake.Extensions;

namespace Snowflake.Controller
{
    public class ControllerProfile : IControllerProfile
    {
        public IReadOnlyDictionary<string, string> InputConfiguration { get { return this.inputConfiguration.AsReadOnly(); } }
        private IDictionary<string, string> inputConfiguration;
        public string ControllerID { get; private set; }
        public string PlatformID { get; private set; }
        public ControllerProfileType ProfileType { get; private set; }
        public ControllerProfile(string controllerId, string platformId, ControllerProfileType profileType, IDictionary<string, string> inputConfiguration)
        {
            this.ControllerID = controllerId;
            this.PlatformID = platformId;
            this.ProfileType = profileType;
            this.inputConfiguration = inputConfiguration;
        }

        public static ControllerProfile GenerateGamepadDefault(ControllerDefinition controllerDefinition, string platformId){
            var controllerId = controllerDefinition.ControllerID;
            var inputConfiguration = new Dictionary<string, string>();
            foreach (var input in controllerDefinition.ControllerInputs)
            {
                inputConfiguration.Add(input.Value.InputName, input.Value.GamepadDefault);
            }
            return new ControllerProfile(controllerId, platformId, ControllerProfileType.GAMEPAD_PROFILE, inputConfiguration);
        }

        public static ControllerProfile FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate){
            string controllerId = protoTemplate["ControllerID"];
            string platformId = protoTemplate["PlatformID"];
            ControllerProfileType profileType = Enum.Parse(typeof(ControllerProfileType), protoTemplate["ProfileType"]);
            Dictionary<string, string> inputConfiguration = 
                ((IDictionary<object, object>)protoTemplate["InputConfiguration"]).ToDictionary(i => (string)i.Key, i =>(string)i.Value);
            return new ControllerProfile(controllerId, platformId, profileType, inputConfiguration);
        }

        public IDictionary<string, dynamic> ToSerializable()
        {
            var serializable = new Dictionary<string, dynamic>();
            serializable["ControllerID"] = this.ControllerID;
            serializable["PlatformID"] = this.PlatformID;
            serializable["InputConfiguration"] = this.inputConfiguration;
            serializable["ProfileType"] = Enum.GetName(typeof(ControllerProfileType), this.ProfileType);
            return serializable;
        }


    }    
}
