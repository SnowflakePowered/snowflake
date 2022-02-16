using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{

    [StructLayout(LayoutKind.Sequential)]
    public struct OverlayTextureEventParams
    {
        public nint TextureHandle;
        public int SourceProcessId;
        public uint Width;
        public uint Height;
        public ulong Size;
        public ulong Alignment;
        public nint SyncHandle;
    }
}
