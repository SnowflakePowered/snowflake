using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Input;
using Snowflake.Extensions;
using Newtonsoft.Json.Linq;
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

        public static IInputTemplate FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate)
        {
            string template = protoTemplate["template"];
            IList<string> templateKeys = protoTemplate["templatekeys"].ToObject<IList<string>>();
            string nobind = protoTemplate["nobind"];
            string name = protoTemplate["name"];

            /*
             * LINQ magic to load gamepad and keyboard mappings
             * Convert the JObjects to a Dictionary of JObjects
             * Convert that Dictionary of JObjects to Dictionary<string,string> and use that to init the mapping object.
             * 
             * Needs improvement but I'm not sure how to improve it.
             */
            IDictionary<string, IGamepadMapping> gamepadMappings = ((JObject)protoTemplate["gamepad"])
                .ToObject<IDictionary<string, JObject>>()
                .ToDictionary(mapping => mapping.Key, mapping => 
                    (IGamepadMapping)new GamepadMapping(mapping.Value.ToObject <IDictionary<string, string>>()));

            IDictionary<string, IKeyboardMapping> keyboardMappings = ((JObject)protoTemplate["keyboard"])
                .ToObject<IDictionary<string, JObject>>()
                .ToDictionary(mapping => mapping.Key, mapping =>
                    (IKeyboardMapping)new KeyboardMapping(mapping.Value.ToObject<IDictionary<string, string>>()));

            return new InputTemplate(name, template, templateKeys, nobind, gamepadMappings, keyboardMappings);
        }

    }
} 
