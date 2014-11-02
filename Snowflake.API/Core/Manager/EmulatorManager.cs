using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Snowflake.Emulator;
using System.IO;
using SharpYaml.Serialization;

namespace Snowflake.Core.Manager
{
    public class EmulatorManager
    {
        private IDictionary<string, dynamic> emulatorCores;
        public IReadOnlyDictionary<string, dynamic> EmulatorCores { get { return this.emulatorCores.AsReadOnly(); } }
        public string LoadablesLocation { get; private set; }
        public EmulatorManager(string loadablesLocation)
        {
            this.emulatorCores = new Dictionary<string, dynamic>();
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
                var emulator = serializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(fileName));
                this.emulatorCores.Add(emulator["id"], emulator);
            }
        }
    }
}
