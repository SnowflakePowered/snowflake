using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Emulator.RetroArch
{
    public class EmulatorRetroArch
    {
        public string MainExecutable { get; private set; }
        public string EmulatorRoot { get; private set; }
        public EmulatorRetroArch()
        {
            this.MainExecutable = "retroarch.exe";
            this.EmulatorRoot = "emulator_retroarch";
        }

        public ProcessStartInfo GetProcessStartInfo(string platformId, string fileName){
            var startInfo = new ProcessStartInfo()
            {
                FileName = Path.Combine(Assembly.GetExecutingAssembly().CodeBase, this.EmulatorRoot,this.MainExecutable),
                Arguments = "-L nestopia_libretro.dll SuperMarioBrothers.nes"
            };

            return startInfo;
        }
        

    }
}
