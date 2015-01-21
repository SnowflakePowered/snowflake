using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input;
using Snowflake.Extensions;

namespace Snowflake.Emulator.Input
{
    public class InputTemplate : IInputTemplate
    {
        public string StringTemplate { get; private set; }
        public IReadOnlyList<string> TemplateKeys { get { return this.templateKeys.AsReadOnly(); } }
        private IList<string> templateKeys;
        public string NoBind { get; private set; }
        public string Name { get; private set; }
        public IReadOnlyDictionary<string, IGamepadMapping> GamepadMappings { get { return this.gamepadMappings.AsReadOnly(); } }
        private IDictionary<string, IGamepadMapping> gamepadMappings;
        public IReadOnlyDictionary<string, IKeyboardMapping> KeyboardMappings { get { return this.keyboardMappings.AsReadOnly(); } }
        private IDictionary<string, IKeyboardMapping> keyboardMappings;


        public InputTemplate(string name, string stringTemplate, IList<string> templateKeys, string noBind, IDictionary<string, IGamepadMapping> gamepadMappings, IDictionary<string, IKeyboardMapping> keyboardMappings)
        {
            this.Name = name;
            this.StringTemplate = stringTemplate;
            this.NoBind = noBind;
            this.gamepadMappings = gamepadMappings;
            this.keyboardMappings = keyboardMappings;
            this.templateKeys = templateKeys;
        }

        public static InputTemplate FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            string template = protoTemplate["template"];
            IList<string> templateKeys = (from key in (IList<object>) protoTemplate["templatekeys"] select (string) key).ToList();
            string nobind = protoTemplate["nobind"];
            string name = protoTemplate["name"];
            IDictionary<string, IGamepadMapping> gamepadMappings = (from mapping in (IDictionary<object, object>)protoTemplate["gamepad"] select mapping)
                .ToDictionary(mapping => (string)mapping.Key, mapping => (IGamepadMapping)new GamepadMapping(((IDictionary<object, object>)mapping.Value)
                    .ToDictionary(input => (string)input.Key, input => (string)input.Value)));

            IDictionary<string, IKeyboardMapping> keyboardMappings = (from mapping in (IDictionary<object, object>)protoTemplate["keyboard"] select mapping)
              .ToDictionary(mapping => (string)mapping.Key, mapping => (IKeyboardMapping)new KeyboardMapping(((IDictionary<object, object>)mapping.Value)
                  .ToDictionary(input => (string)input.Key, input => (string)input.Value)));

            return new InputTemplate(name, template, templateKeys, nobind, gamepadMappings, keyboardMappings);
        }

    }
} 
