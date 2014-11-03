using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Emulator;
using Snowflake.Core.Manager;
using System.Diagnostics;
using System.IO;

namespace Snowflake.Extensions
{
    public static class EmulatorManagerExts
    {
        public static ProcessStartInfo GetExecutableEmulatorProcess(this EmulatorManager manager, EmulatorCore core, string parameters = "")
        {
            if (core.AssemblyType != EmulatorAssemblyType.EMULATOR_EXECUTABLE)
                throw new InvalidOperationException("You can only run executable emulators");
            return new ProcessStartInfo()
            {
                FileName = Path.Combine(manager.LoadablesLocation, core.EmulatorId, core.MainAssembly),
                Arguments = parameters,
                WorkingDirectory = Path.Combine(manager.LoadablesLocation, core.EmulatorId)
            };
        }
    }
}
