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
        public ControllerProfileType ProfileType { get; private set; }
        public ControllerProfile(string controllerId, ControllerProfileType profileType, IDictionary<string, string> inputConfiguration)
        {
            this.ControllerID = controllerId;
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
            return new ControllerProfile(controllerId, ControllerProfileType.GAMEPAD_PROFILE, inputConfiguration);
        }

        public static ControllerProfile FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate){
            string controllerId = protoTemplate["ControllerID"];
            ControllerProfileType profileType = Enum.Parse(typeof(ControllerProfileType), protoTemplate["ProfileType"]);
            Dictionary<string, string> inputConfiguration = 
                ((IDictionary<object, object>)protoTemplate["InputConfiguration"].ToObject<IDictionary<object, object>>()).ToDictionary(i => (string)i.Key, i =>(string)i.Value);
            return new ControllerProfile(controllerId, profileType, inputConfiguration);
        }

        public IDictionary<string, dynamic> ToSerializable()
        {
            var serializable = new Dictionary<string, dynamic>();
            serializable["ControllerID"] = this.ControllerID;
            serializable["InputConfiguration"] = this.inputConfiguration;
            serializable["ProfileType"] = Enum.GetName(typeof(ControllerProfileType), this.ProfileType);
            return serializable;
        }


    }    
}
