using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Snowflake.Extensions;

namespace Snowflake.Platform.Controller
{
    public class ControllerDefinition
    {
        public ReadOnlyDictionary<string, ControllerInput> ControllerInputs { get { return this.controllerInputs.AsReadOnly(); } }
        private IDictionary<string, ControllerInput> controllerInputs;
        public string ControllerID { get; private set; }
        public ControllerDefinition(IDictionary<string, ControllerInput> controllerInputs, string controllerId)
        {
            this.ControllerID = controllerId;
            this.controllerInputs = controllerInputs;
        }
    }
}
