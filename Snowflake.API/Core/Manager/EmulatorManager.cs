using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Snowflake.Emulator;
using Snowflake.Emulator.Core.Mapping;
using System.IO;
using SharpYaml.Serialization;

namespace Snowflake.Core.Manager
{
    public class EmulatorManager
    {
        private IDictionary<string, EmulatorCore> emulatorCores;
        public IReadOnlyDictionary<string, EmulatorCore> EmulatorCores { get { return this.emulatorCores.AsReadOnly(); } }
        public string LoadablesLocation { get; private set; }
        public EmulatorManager(string loadablesLocation)
        {
            this.emulatorCores = new Dictionary<string, EmulatorCore>();
            this.LoadablesLocation = loadablesLocation;
        }

        public void LoadEmulatorCores()
        { 
            var serializer = new Serializer(new SerializerSettings()
            {
                EmitTags = false
            });
            foreach (string fileName in Directory.GetFiles(this.LoadablesLocation))
            {
                if (!(Path.GetExtension(fileName) == ".yml")) continue;
                var emulator = serializer.Deserialize<Dictionary<string, dynamic>>(File.ReadAllText(fileName));

                var gameMappings = new Dictionary<string, GamepadMapping>();
                foreach (var value in emulator["gamepad"])
                {
                    var mappingDictionary = (Dictionary<object, object>) value.Value;
                    gameMappings.Add(value.Key, new GamepadMapping(mappingDictionary.ToDictionary(k => k.Key.ToString(), k => k.Value.ToString())));
                }
                var booleanMapping = new BooleanMapping(emulator["boolean"][true], emulator["boolean"][false]);
                var emulatorCore = new EmulatorCore(emulator["main"], emulator["id"], emulator["name"], emulator["type"], gameMappings, booleanMapping);
                this.emulatorCores.Add(emulator["id"], emulatorCore);
            }
        }

       
    }
}
