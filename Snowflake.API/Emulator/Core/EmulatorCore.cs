using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Emulator
{
    public class EmulatorCore
    {
        public string RootExecutable { get; private set; }
        public EmulatorCore(string rootExecutable)
        {
            this.RootExecutable = rootExecutable;
        }
    }
}
