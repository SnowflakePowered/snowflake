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
        public IDictionary<string, string> InputConfiguration { get; private set; }
        public string ControllerID { get; private set; }
        public ControllerProfileType ProfileType { get; private set; }
        public ControllerProfile(string controllerId, ControllerProfileType profileType, IDictionary<string, string> inputConfiguration)
        {
            this.ControllerID = controllerId;
            this.ProfileType = profileType;
            this.InputConfiguration = inputConfiguration;
        }

        public static ControllerProfile FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate){
            string controllerId = protoTemplate["ControllerID"];
            ControllerProfileType profileType = Enum.Parse(typeof(ControllerProfileType), protoTemplate["ProfileType"]);
            Dictionary<string, string> inputConfiguration = 
                ((IDictionary<object, object>)protoTemplate["InputConfiguration"].ToObject<IDictionary<object, object>>()).ToDictionary(i => (string)i.Key, i =>(string)i.Value);
            return new ControllerProfile(controllerId, profileType, inputConfiguration);
        }

        public IDictionary<string, object> ToSerializable()
        {
            var serializable = new Dictionary<string, object>();
            serializable["ControllerID"] = this.ControllerID;
            serializable["InputConfiguration"] = this.InputConfiguration;
            serializable["ProfileType"] = Enum.GetName(typeof(ControllerProfileType), this.ProfileType);
            return serializable;
        }


    }    
}
