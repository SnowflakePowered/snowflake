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

        public ControllerTemplate(string controllerId, string emulatorId, string platformId, IDictionary<string, ControllerMapping> keyboardControllerMappings, IDictionary<string, ControllerMapping> gamepadControllerMappings)
        {
            this.ControllerID = controllerId;
            this.EmulatorID = emulatorId;
            this.PlatformID = platformId;
            this.keyboardControllerMappings = keyboardControllerMappings;
            this.gamepadControllerMappings = gamepadControllerMappings;
        }
        public static ControllerTemplate FromDictionary(IDictionary<string, dynamic> protoTemplate)
        {
            var controllerid = protoTemplate["controller"];
            var emulator = protoTemplate["emulator"];
            var platformid = protoTemplate["platform"];
            var gamepadControllerMappings = new Dictionary<string, ControllerMapping>();
            var keyboardControllerMappings = new Dictionary<string, ControllerMapping>();

            foreach (var mapping in protoTemplate["gamepad"])
            {
                var name = mapping.Key;
                IDictionary<string, string> keyMappings = (from keys in (IDictionary<object, object>)mapping.Value["keys"] select keys)
                    .ToDictionary(keys => (string)keys.Key, keys => (string)keys.Value);
                IDictionary<string, string> inputMappings = (from keys in (IDictionary<object, object>)mapping.Value["input"] select keys)
                    .ToDictionary(keys => (string)keys.Key, keys => (string)keys.Value);
                gamepadControllerMappings.Add(name, new ControllerMapping(ControllerMappingType.GAMEPAD_MAPPING, keyMappings, inputMappings));
            }

            foreach (var mapping in protoTemplate["keyboard"])
            {
                var name = mapping.Key;
                IDictionary<string, string> keyMappings = (from keys in (IDictionary<object, object>)mapping.Value["keys"] select keys)
                    .ToDictionary(keys => (string)keys.Key, keys => (string)keys.Value);
                IDictionary<string, string> inputMappings = (from keys in (IDictionary<object, object>)mapping.Value["input"] select keys)
                    .ToDictionary(keys => (string)keys.Key, keys => (string)keys.Value);
                keyboardControllerMappings.Add(name, new ControllerMapping(ControllerMappingType.KEYBOARD_MAPPING, keyMappings, inputMappings));
            }

            return new ControllerTemplate(controllerid, emulator, platformid, keyboardControllerMappings, gamepadControllerMappings);
        }
    }
}
