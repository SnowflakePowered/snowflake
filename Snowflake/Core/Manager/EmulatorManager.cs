using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Extensions;
using Snowflake.Emulator;
using System.IO;
using SharpYaml.Serialization;

namespace Snowflake.Service.Manager
{
    public class EmulatorManager
    {
        private IDictionary<string, IEmulatorAssembly> emulatorAssemblies;
        public IReadOnlyDictionary<string, IEmulatorAssembly> EmulatorAssemblies { get { return this.emulatorAssemblies.AsReadOnly(); } }
        public string AssembliesLocation { get; private set; }
        public EmulatorManager(string assembliesLocation)
        {
            this.emulatorAssemblies = new Dictionary<string, IEmulatorAssembly>();
            this.AssembliesLocation = assembliesLocation;
        }

        public void LoadEmulatorAssemblies()
        { 
            foreach (string fileName in Directory.GetFiles(this.AssembliesLocation))
            {
                if (!(Path.GetExtension(fileName) == ".yml")) continue;
                
                var emulatorCore = EmulatorManager.ParseEmulatorAssembly(fileName);
                this.emulatorAssemblies.Add(emulatorCore.EmulatorId, emulatorCore);
            }
        }
        public string GetAssemblyDirectory(IEmulatorAssembly assembly){
            return Path.Combine(this.AssembliesLocation, assembly.EmulatorId, assembly.EmulatorName);
        }
        public static IEmulatorAssembly ParseEmulatorAssembly(string emulatorCorePath)
        {
            var emulator = new Serializer().Deserialize<Dictionary<string, dynamic>>(File.ReadAllText(emulatorCorePath));
            return EmulatorAssembly.FromDictionary(emulator);
        }

       
    }
}
