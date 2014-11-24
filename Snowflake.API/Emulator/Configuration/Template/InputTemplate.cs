using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;
using Snowflake.Extensions;

namespace Snowflake.Emulator.Configuration.Template
{
    public class InputTemplate
    {
        public string StringTemplate { get; private set; }
        public string NoBind { get; private set; }
        public IReadOnlyDictionary<string, GamepadMapping> GamepadMappings { get { return this.gamepadMappings.AsReadOnly(); } }
        private IDictionary<string, GamepadMapping> gamepadMappings;
        public IReadOnlyDictionary<string, KeyboardMapping> KeyboardMappings { get { return this.keyboardMappings.AsReadOnly(); } }
        private IDictionary<string, KeyboardMapping> keyboardMappings;

        public InputTemplate(string stringTemplate, string NoBind);

    }
}
