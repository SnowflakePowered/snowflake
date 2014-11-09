using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator.Configuration.Mapping;

namespace Snowflake.Emulator
{
    public class EmulatorCore
    {
        public string MainAssembly { get; private set; }
        public string EmulatorId { get; private set; }
        public string EmulatorName { get; private set; }
        public EmulatorAssemblyType AssemblyType { get; private set; }

        public EmulatorCore(string mainAssembly, string emulatorId, string name, string assemblyTypeString)
        {
            EmulatorAssemblyType assemblyType;
            if (!Enum.TryParse<EmulatorAssemblyType>(assemblyTypeString, true, out assemblyType)) assemblyType = EmulatorAssemblyType.EMULATOR_MISC;
            this.MainAssembly = mainAssembly;
            this.EmulatorId = emulatorId;
            this.EmulatorName = name;
            this.AssemblyType = assemblyType;


        }

        public EmulatorCore(string mainAssembly, string emulatorId, string name, EmulatorAssemblyType assemblyType)
        {
            this.MainAssembly = mainAssembly;
            this.EmulatorId = emulatorId;
            this.EmulatorName = name;
            this.AssemblyType = assemblyType;
        }
    }
    public enum EmulatorAssemblyType
    {
        EMULATOR_EXECUTABLE,
        EMULATOR_LIBRARY,
        EMULATOR_MISC
    }
}
