using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Snowflake.Emulator.Input.Mapping;
namespace Snowflake.Emulator.Input.Template
{
    public class ControllerTemplate
    {
        public string ControllerID { get; private set; }
        public string EmulatorID { get; private set; }
        public string PlatformID { get; private set; }

        public IReadOnlyDictionary<string, ControllerMapping> KeyboardControllerMappings { get { return this.keyboardControllerMappings.AsReadOnly(); } }
        private IDictionary<string, ControllerMapping> keyboardControllerMappings;
        public IReadOnlyDictionary<string, ControllerMapping> GamepadControllerMappings { get { return this.gamepadControllerMappings.AsReadOnly(); } }
        private IDictionary<string, ControllerMapping> gamepadControllerMappings;

        public static void FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            var controllerid = protoTemplate["controller"];
            var emulator = protoTemplate["emulator"];
            var platformid = protoTemplate["platform"];
            
        }
    }
}
