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
        public IReadOnlyList<string> TemplateKeys { get { return this.templateKeys.AsReadOnly(); } }
        private IList<string> templateKeys;
        public string NoBind { get; private set; }
        public IReadOnlyDictionary<string, GamepadMapping> GamepadMappings { get { return this.gamepadMappings.AsReadOnly(); } }
        private IDictionary<string, GamepadMapping> gamepadMappings;
        public IReadOnlyDictionary<string, KeyboardMapping> KeyboardMappings { get { return this.keyboardMappings.AsReadOnly(); } }
        private IDictionary<string, KeyboardMapping> keyboardMappings;


        public InputTemplate(string stringTemplate, IList<string> templateKeys, string noBind, IDictionary<string, GamepadMapping> gamepadMappings, IDictionary<string, KeyboardMapping> keyboardMappings)
        {
            this.StringTemplate = stringTemplate;
            this.NoBind = noBind;
            this.gamepadMappings = gamepadMappings;
            this.keyboardMappings = keyboardMappings;
            this.templateKeys = templateKeys;
        }

        public static void FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            string template = protoTemplate["template"];
            IList<string> templateKeys = (from key in (IList<object>) protoTemplate["templatekeys"] select (string) key).ToList();
            string nobind = protoTemplate["nobind"];
        
            IDictionary<string, GamepadMapping> gamepadMappings = (from mapping in (IDictionary<object, object>)protoTemplate["gamepad"] select mapping)
                .ToDictionary(mapping => (string)mapping.Key, mapping => new GamepadMapping(((IDictionary<object, object>)mapping.Value)
                    .ToDictionary(input => (string) input.Key, input => (string) input.Value)));
            IDictionary<string, KeyboardMapping> keyboardMappings = (from mapping in (IDictionary<object, object>)protoTemplate["keyboard"] select mapping)
              .ToDictionary(mapping => (string)mapping.Key, mapping => new KeyboardMapping(((IDictionary<object, object>)mapping.Value)
                  .ToDictionary(input => (string)input.Key, input => (string)input.Value)));
              
        }

    }
} 
