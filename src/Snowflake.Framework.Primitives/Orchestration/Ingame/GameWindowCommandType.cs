using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{
    public enum GameWindowCommandType : byte
    {
        Handshake = 1,
        WindowResizeEvent = 2,
        WindowMessageEvent = 3,
        MouseEvent = 4,
        CursorEvent = 5,
        OverlayTextureEvent = 6,
        ShutdownEvent = 7,
    }
}
