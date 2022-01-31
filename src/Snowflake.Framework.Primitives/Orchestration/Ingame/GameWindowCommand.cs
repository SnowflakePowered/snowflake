using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Orchestration.Ingame
{


    [StructLayout(LayoutKind.Explicit, Pack = 0)]
    public struct GameWindowCommand
    {
        public const byte GameWindowMagic = 0x9F;

        [FieldOffset(0)] public byte Magic;
        [FieldOffset(1)] public GameWindowCommandType Type;
        [FieldOffset(6)] public HandshakeEventParams HandshakeEvent;
        [FieldOffset(6)] public WindowResizeEventParams ResizeEvent;
        [FieldOffset(6)] public WindowMessageEventParams WindowMessageEvent;
        [FieldOffset(6)] public MouseEventParams MouseEvent;
        [FieldOffset(6)] public CursorEventParams CursorEvent;
        [FieldOffset(6)] public OverlayTextureEventParams TextureEvent;

        public ReadOnlyMemory<byte> ToBuffer()
        {
            return StructUtils.ToMemory(this);
        }

        public static GameWindowCommand? FromBuffer(ReadOnlyMemory<byte> buffer)
        {
            return StructUtils.FromSpan<GameWindowCommand>(buffer);
        }

        public static GameWindowCommand Handshake(Guid id)
        {
            return new()
            {
                Magic = GameWindowMagic,
                Type = GameWindowCommandType.Handshake,
                HandshakeEvent = new()
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

                Memory<byte> _bytes = new byte[Marshal.SizeOf<GameWindowCommand>()];
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
}
