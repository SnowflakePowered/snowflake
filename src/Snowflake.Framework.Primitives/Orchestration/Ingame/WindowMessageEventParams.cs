using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{

    [StructLayout(LayoutKind.Sequential)]
    public struct WindowMessageEventParams
    {
        public int Message;
        public ulong WParam;
        public int LParam;
    }
}
