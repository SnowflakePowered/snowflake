using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ComponentModel.Composition;
using Snowflake.API.Base.Emulator;

namespace Emulator.RetroArch
{
    [Export]
    public class EmulatorRetroArch : ExecutableEmulator
    {
        public string MainExecutable { get; private set; }
        public string EmulatorRoot { get; private set; }
        public EmulatorRetroArch() : base(Assembly.GetExecutingAssembly())
        {
            this.MainExecutable = this.PluginInfo["emulator_executable"];
            this.EmulatorRoot = this.PluginInfo["emulator_root"];
            this.InitConfiguration();
            Console.WriteLine(this.PluginConfiguration.Configuration["cores"]["NINTENDO_NES"]);
        }

        public override void Run(string uuid)
        {
            throw new NotImplementedException();
        }
        public void Run(string platformId, string fileName)
        {
            Process.Start(GetProcessStartInfo(platformId, fileName));
        }
        public ProcessStartInfo GetProcessStartInfo(string platformId, string fileName){
            var startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), this.EmulatorRoot,this.MainExecutable),
                Arguments = @"-L cores\"+this.PluginConfiguration.Configuration["cores"][platformId]+" "+fileName,
                WorkingDirectory = this.EmulatorRoot
            };
            return startInfo;
        }
        

    }
}
