using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
namespace Snowflake.Emulator.Input.Mapping
{
    public class ControllerMapping
    {
        public ControllerMappingType MappingType { get; private set; }
        public IReadOnlyDictionary<string, string> KeyMappings { get { return this.keyMappings.AsReadOnly(); } }
        private IDictionary<string, string> keyMappings;
        public IReadOnlyDictionary<string, string> InputMappings { get { return this.inputMappings.AsReadOnly(); } }
        private IDictionary<string, string> inputMappings;

        public ControllerMapping(ControllerMappingType mappingType, IDictionary<string, string> keyMappings, IDictionary<string, string> inputMappings)
        {
            this.MappingType = mappingType;
            this.keyMappings = keyMappings;
            this.inputMappings = inputMappings;
        }
    }
    public enum ControllerMappingType
    {
        GAMEPAD_MAPPING,
        KEYBOARD_MAPPING
    }

}
