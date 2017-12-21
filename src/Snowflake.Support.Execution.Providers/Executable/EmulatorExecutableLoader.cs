using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Execution.Process;
using Snowflake.Extensibility;
using Snowflake.Loader;

namespace Snowflake.Support.Execution
{
    public class EmulatorExecutableLoader : IModuleLoader<IEmulatorExecutable>
    {
        private ILogger Logger { get; }
        public EmulatorExecutableLoader(ILogger logger)
        {
            this.Logger = logger;
        }

        public IEnumerable<IEmulatorExecutable> LoadModule(IModule module)
        {
            FileInfo emulatorExecutable = new FileInfo(Path.Combine(module.ContentsDirectory.FullName, module.Entry));
            if (!emulatorExecutable.Exists)
            {
                throw new FileNotFoundException($"Emulator Entry {emulatorExecutable.FullName} does not exist!");
            }

            this.Logger.Info($"Loading emulator { module.Name } {module.Version}");
            yield return new EmulatorExecutable(emulatorExecutable, module.Name, module.Version);
        }
    }
}
