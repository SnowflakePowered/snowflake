using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
namespace Snowflake.Emulator.Input
{
    public class ControllerTemplate : IControllerTemplate
    {
        public string ControllerID { get; private set; }
        public string InputTemplateName { get; private set; }
        public string PlatformID { get; private set; }

        public IReadOnlyDictionary<string, IControllerMapping> KeyboardControllerMappings { get { return this.keyboardControllerMappings.AsReadOnly(); } }
        private IDictionary<string, IControllerMapping> keyboardControllerMappings;
        public IReadOnlyDictionary<string, IControllerMapping> GamepadControllerMappings { get { return this.gamepadControllerMappings.AsReadOnly(); } }
        private IDictionary<string, IControllerMapping> gamepadControllerMappings;

        public ControllerTemplate(string controllerId, string emulatorId, string platformId, IDictionary<string, IControllerMapping> keyboardControllerMappings, IDictionary<string, IControllerMapping> gamepadControllerMappings)
        {
            this.ControllerID = controllerId;
            this.InputTemplateName = emulatorId;
            this.PlatformID = platformId;
            this.keyboardControllerMappings = keyboardControllerMappings;
            this.gamepadControllerMappings = gamepadControllerMappings;
        }
        public static IControllerTemplate FromJsonProtoTemplate(IDictionary<string, dynamic> protoTemplate)
        {
            var controllerid = protoTemplate["controller"];
            var inputtemplate = protoTemplate["input_template"];
            var platformid = protoTemplate["platform"];
            var gamepadControllerMappings = new Dictionary<string, IControllerMapping>();
            var keyboardControllerMappings = new Dictionary<string, IControllerMapping>();

            foreach (var mapping in protoTemplate["gamepad"])
            {
                var name = mapping.Name;
                IDictionary<string, string> keyMappings = new Dictionary<string, string>();
                IDictionary<string, string> inputMappings = new Dictionary<string, string>();
                foreach(var keyMapping in mapping.Value.keys)
                {
                    keyMappings.Add(keyMapping.Name, keyMapping.Value.Value);
                }

                foreach (var keyMapping in mapping.Value.input)
                {
                    inputMappings.Add(keyMapping.Name, keyMapping.Value.Value);
                }
                gamepadControllerMappings.Add(name, new ControllerMapping(ControllerMappingType.GAMEPAD_MAPPING, keyMappings, inputMappings));
            }

            foreach (var mapping in protoTemplate["keyboard"])
            {
                var name = mapping.Name;
                IDictionary<string, string> keyMappings = new Dictionary<string, string>();
                IDictionary<string, string> inputMappings = new Dictionary<string, string>();
                foreach (var keyMapping in mapping.Value.keys)
                {
                    keyMappings.Add(keyMapping.Name, keyMapping.Value.Value);
                }
                foreach (var keyMapping in mapping.Value.input)
                {
                    inputMappings.Add(keyMapping.Name, keyMapping.Value.Value);
                }
                keyboardControllerMappings.Add(name, new ControllerMapping(ControllerMappingType.KEYBOARD_MAPPING, keyMappings, inputMappings));
            }

            return new ControllerTemplate(controllerid, inputtemplate, platformid, keyboardControllerMappings, gamepadControllerMappings);
        }
    }
}
