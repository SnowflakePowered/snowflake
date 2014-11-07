using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Core.Mapping;

namespace Snowflake.Emulator
{
    public class EmulatorCore
    {
        public string MainAssembly { get; private set; }
        public string EmulatorId { get; private set; }
        public string EmulatorName { get; private set; }
        public EmulatorAssemblyType AssemblyType { get; private set; }
        public IDictionary<string, GamepadMapping> GamepadMappings { get; private set; }
        public BooleanMapping BooleanMapping { get; private set; }

        public EmulatorCore(string mainAssembly, string emulatorId, string name, string assemblyTypeString, IDictionary<string, GamepadMapping> gamepadMappings, BooleanMapping booleanMapping)
        {
            EmulatorAssemblyType assemblyType;
            if (!Enum.TryParse<EmulatorAssemblyType>(assemblyTypeString, true, out assemblyType)) assemblyType = EmulatorAssemblyType.EMULATOR_MISC;
            this.MainAssembly = mainAssembly;
            this.EmulatorId = emulatorId;
            this.EmulatorName = name;
            this.AssemblyType = assemblyType;
            this.GamepadMappings = gamepadMappings;
            this.BooleanMapping = booleanMapping;

        }

        public EmulatorCore(string mainAssembly, string emulatorId, string name, EmulatorAssemblyType assemblyType, IDictionary<string, GamepadMapping> gamepadMappings, BooleanMapping booleanMapping)
        {
            this.MainAssembly = mainAssembly;
            this.EmulatorId = emulatorId;
            this.EmulatorName = name;
            this.AssemblyType = assemblyType;
            this.GamepadMappings = gamepadMappings;
            this.BooleanMapping = booleanMapping;
        }
    }
    public enum EmulatorAssemblyType
    {
        EMULATOR_EXECUTABLE,
        EMULATOR_LIBRARY,
        EMULATOR_MISC
    }
}
