using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WindowResizeEventParams
    {
        public int Height;
        public int Width;
        public byte Force;
    }
}
