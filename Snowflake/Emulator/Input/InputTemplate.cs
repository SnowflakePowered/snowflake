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
        public string StringTemplate { get; }
        public IReadOnlyList<string> TemplateKeys => this.templateKeys.AsReadOnly();
        private IList<string> templateKeys;
        public string NoBind { get; }
        public string Name { get; }
        public IReadOnlyDictionary<string, IGamepadMapping> GamepadMappings => this.gamepadMappings.AsReadOnly();
        private IDictionary<string, IGamepadMapping> gamepadMappings;
        public IReadOnlyDictionary<string, IKeyboardMapping> KeyboardMappings => this.keyboardMappings.AsReadOnly();
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

        public static IInputTemplate FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate, EmulatorBridge bridge)
        {
            IList<string> templateKeys = protoTemplate["templatekeys"].ToObject<IList<string>>();
            string nobind = protoTemplate["nobind"];
            string name = protoTemplate["name"];
            string template = bridge.GetStringResource($"{name}.template");
            IDictionary<string, IGamepadMapping> gamepadMappings = new Dictionary<string, IGamepadMapping>();
            IDictionary<string, IKeyboardMapping> keyboardMappings = new Dictionary<string, IKeyboardMapping>();

            foreach(var gamepadMapping in protoTemplate["gamepad"])
            {
                gamepadMappings.Add(gamepadMapping.Name, (IGamepadMapping)new GamepadMapping(gamepadMapping.Value.ToObject<IDictionary<string, string>>()));
            }
            foreach (var keyboardMapping in protoTemplate["keyboard"])
            {
                 keyboardMappings.Add(keyboardMapping.Name, (IKeyboardMapping)new KeyboardMapping(keyboardMapping.Value.ToObject<IDictionary<string, string>>()));
            }

            return new InputTemplate(name, template, templateKeys, nobind, gamepadMappings, keyboardMappings);
        }

    }
} 
