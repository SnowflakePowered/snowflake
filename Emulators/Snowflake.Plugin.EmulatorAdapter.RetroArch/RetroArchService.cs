using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch
{
    //todo refactor into plugin
    internal class RetroArchService
    {
        public ProcessStartInfo GetRetroArchStartInfo(string core, RetroArchInstance instance)
        {
            //assume in retroarch folder
            return new ProcessStartInfo(Path.Combine("retroarch", "retroarch.exe"))
            {
                Arguments = $"-L {core}",
                WorkingDirectory = instance.InstancePath
            };
        }
    }
}
