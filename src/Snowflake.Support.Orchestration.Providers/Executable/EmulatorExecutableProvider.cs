using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Orchestration.Process;
using Snowflake.Extensibility;
using Snowflake.Loader;

namespace Snowflake.Support.Execution
{
    public class EmulatorExecutableProvider : IEmulatorExecutableProvider
    {
        private IList<IEmulatorExecutable> Executables { get; }

        public EmulatorExecutableProvider(ILogger logger, IModuleEnumerator enumerator)
        {
            var loader = new EmulatorExecutableLoader(logger);
            this.Executables = enumerator.Modules
                .Where(m => m.Loader == "emulator")
                .SelectMany(m => loader.LoadModule(m)).ToList();
        }

        public IEmulatorExecutable GetEmulator(string name)
        {
            return this.Executables.FirstOrDefault(e => e.EmulatorName == name);
        }

        public IEmulatorExecutable GetEmulator(string name, Version semver)
        {
            return this.Executables.FirstOrDefault(e => e.EmulatorName == name
                                                        && e.Version.Major == semver.Major);
        }
    }
}
