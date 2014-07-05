using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Base.Emulator
{
    public class EmulatorSettings
    {
        public string PrefferedEmulator { get; private set; }
        public object GameSettings { get; private set; }
        public EmulatorSettings(string preferredEmulator = "default", object gameSettings=null)
        {
            this.PrefferedEmulator = preferredEmulator;
            this.GameSettings = gameSettings;
        }
    }
}
