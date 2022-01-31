using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseEventParams
    {
        public MouseButton MouseDoubleClick;

        public MouseButton MouseDown;
        public MouseButton MouseUp;
        public ModifierKeys Modifiers;

        public float MouseX;
        public float MouseY;
        public float WheelX;
        public float WheelY;
    }

    [Flags]
    public enum ModifierKeys : byte
    {
        None = 0,
        Shift = 1 << 0,
        Control = 1 << 1,
        Alt = 1 << 2
    }

    [Flags]
    public enum MouseButton : byte
    {
        None = 0,
        Mouse1 = 1 << 0,
        Mouse2 = 1 << 1,
        Mouse3 = 1 << 2,
        Mouse4 = 1 << 3,
        Mouse5 = 1 << 4
    }
}
