using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    internal class RetroArchProcess 
    {

        public string ConfigPath { private get; set; }
        public string CorePath { private get; set; }
        public string SavePath { private get; set; }
        public string Subsystem { private get; set; }
        private readonly string retroArchDirectory;
        private readonly string romFile;
        public RetroArchProcess(string retroArchDirectory, string romFile)
        {
            this.retroArchDirectory = retroArchDirectory;
            this.romFile = romFile;
        }

        public ProcessStartInfo GetStartInfo()
        {
            return new ProcessStartInfo(Path.Combine(this.retroArchDirectory, "retroarch"))
            {
                WorkingDirectory = this.retroArchDirectory, 
                Arguments = $@"{this.GetArgument(this.SavePath, "-s")}{this.GetArgument(this.ConfigPath, "-c")}{this.GetArgument(this.CorePath, "-L")}""{this.romFile}"" {this.GetArgument(this.Subsystem, "--subsystem")}",
            };
        }

        internal string GetArgument(string value, string argumentName)
        {
            return value != null ? $@"{argumentName} ""{value} """ : "";
        }
    }
}
