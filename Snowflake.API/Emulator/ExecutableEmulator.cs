using System.Diagnostics;
using System.IO;
using System.Reflection;
using Snowflake.Core;
using Snowflake.Information.Game;
using Snowflake.Plugin;

namespace Snowflake.Emulator
{
    public abstract class ExecutableEmulator : BasePlugin, IEmulator
    {
        public string MainExecutable { get; private set; }
        public string EmulatorRoot { get; private set; }

        public ExecutableEmulator(Assembly pluginAssembly):base(pluginAssembly)
        {
            this.MainExecutable = this.PluginInfo["emulator_executable"];
            this.EmulatorRoot = this.PluginInfo["emulator_root"];
        }

        public virtual void Run(string gameUuid)
        {
            Game game = FrontendCore.LoadedCore.GameDatabase.GetGameByUUID(gameUuid);
            this.Run(game.PlatformId, game.FileName);
        }

        public virtual void Run(string platformId, string fileName)
        {
            Process.Start(this.GetProcessStartInfo(platformId, fileName));
        }

        protected abstract ProcessStartInfo GetProcessStartInfo(string platformId, string fileName);
        protected virtual ProcessStartInfo GetProcessStartInfo(string platformId, string fileName, string processArguments)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(Path.GetDirectoryName(PluginAssembly.CodeBase), this.EmulatorRoot, this.MainExecutable),
                Arguments = processArguments,
                WorkingDirectory = this.EmulatorRoot
            };
            return startInfo;
        }
   }
}


