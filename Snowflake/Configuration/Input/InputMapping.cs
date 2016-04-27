using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;

namespace Snowflake.Configuration.Input
{
    public class InputMapping : IInputMapping
    {
        private readonly IDictionary<ControllerElement, string> elementMappings;
        private readonly IDictionary<KeyboardKey, string> keyMappings;
        private readonly string nullMapping;

        public string this[ControllerElement element] => this.elementMappings.ContainsKey(element) ? this.elementMappings[element] : this.nullMapping;

        public string this[KeyboardKey key]
            => this.keyMappings.ContainsKey(key) ? this.keyMappings[key] : this.nullMapping;

        public InputMapping(IDictionary<ControllerElement, string> elementMappings,
            IDictionary<KeyboardKey, string> keyMappings, string nullMapping)
        {
            this.elementMappings = elementMappings;
            this.keyMappings = keyMappings;
            this.nullMapping = nullMapping;
        }
    }
}
