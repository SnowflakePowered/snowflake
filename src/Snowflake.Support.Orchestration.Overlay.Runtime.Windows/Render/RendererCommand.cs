using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.Orchestration.Overlay.Runtime.Windows.Render
{
    public enum RendererCommandType : byte
    {
        Handshake = 1,
        SharedTextureHandle = 2,
        WndProcEvent = 3,
        MouseEvent = 4,
        CursorEvent = 5,
        ResizeEvent = 6,
        ShutdownEvent = 7,
    }

    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct RendererCommand
    {
        public const byte RendererMagic = 0x9F;

        [FieldOffset(0)] public byte Magic;
        [FieldOffset(1)] public RendererCommandType Type;
        [FieldOffset(6)] public HandshakeParams HandshakeParams;
        [FieldOffset(6)] public SharedTextureHandleParams SharedTextureParams;
        [FieldOffset(6)] public WndProcEvent WndProcMessageEvent;
        [FieldOffset(6)] public MouseEventParams MouseEvent;
        [FieldOffset(6)] public ResizeParams ResizeEvent;

        public ReadOnlyMemory<byte> ToBuffer()
        {
            return StructUtils.ToMemory(this);
        }

        public static RendererCommand? FromBuffer(ReadOnlyMemory<byte> buffer)
        {
            return StructUtils.FromSpan<RendererCommand>(buffer);
        }

        public static RendererCommand Handshake(Guid id)
        {
            return new()
            {
                Magic = RendererMagic,
                Type = RendererCommandType.Handshake,
                HandshakeParams = new()
                {
                    Guid = id,
                }
            };
        }

        private static class StructUtils
        {
            public static unsafe ReadOnlyMemory<byte> ToMemory<T>(T value) where T : unmanaged
            {
                byte* pointer = (byte*)&value;

                Memory<byte> _bytes = new byte[Marshal.SizeOf<RendererCommand>()];
                Span<byte> bytes = _bytes.Span;

                for (int i = 0; i < sizeof(T); i++)
                {
                    bytes[i] = pointer[i];
                }

                return _bytes;
            }

            public static unsafe T? FromSpan<T>(ReadOnlyMemory<byte> value) where T : unmanaged
            {
                if (value.Length != Marshal.SizeOf<T>())
                {
                    Console.WriteLine("Expected size " + Marshal.SizeOf<T>() + " but got " + value.Length);
                    return null;
                }

                return MemoryMarshal.Cast<byte, T>(value.Span)[0];
            }
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct HandshakeParams
    {
        public Guid Guid;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SharedTextureHandleParams
    {
        public nint TextureHandle;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WndProcEvent
    {
        public int Message;
        public ulong WParam;
        public int LParam;
    }

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

    [StructLayout(LayoutKind.Sequential)]
    public struct ResizeParams
    {
        public int Height;
        public int Width;
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
