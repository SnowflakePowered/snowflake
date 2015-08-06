using System.Collections.Generic;
using System.Linq;
using Snowflake.Extensions;

namespace Snowflake.Controller
{
    public class ControllerDefinition : IControllerDefinition
    {
        public IReadOnlyDictionary<string, IControllerInput> ControllerInputs => this.controllerInputs.AsReadOnly();
        private readonly IDictionary<string, IControllerInput> controllerInputs;
        public string ControllerID { get; }
        public ControllerDefinition(IDictionary<string, IControllerInput> controllerInputs, string controllerId)
        {
            this.ControllerID = controllerId;
            this.controllerInputs = controllerInputs;
        }
        public static IControllerDefinition FromJsonProtoTemplate(IDictionary<string, dynamic> jsonDictionary)
        {
 
            return new ControllerDefinition(
                   ((IDictionary<string, ControllerInput>)jsonDictionary["ControllerInputs"].ToObject<IDictionary<string, ControllerInput>>())
                        .ToDictionary(input => input.Key, input => (IControllerInput)input.Value), //explicitly cast the ControllerInput to the interface
                   jsonDictionary["ControllerID"]
                );
        }

    }
}
