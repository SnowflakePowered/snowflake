using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Interface;
using Snowflake.API.Base;

namespace Snowflake.API.Base.Emulator
{
    public abstract class ExecutableEmulator : Plugin, IEmulator
    {
        public ExecutableEmulator(string pluginName, string baseDirectory, string executableName):base(pluginName)
        {
        }
        public abstract void Run(string uuid);


   }
}


