/*
Soft64 - C# N64 Emulator
Copyright (C) Soft64 Project @ Codeplex
Copyright (C) 2013 - 2014 Bryan Perris

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>
*/

using System;
using System.IO;

namespace Soft64.IO
{
    public abstract class ByteSwapStream : Stream
    {
        private Stream m_BaseStream;

        protected ByteSwapStream(Stream baseStream)
        {
            if (baseStream == null)
                throw new ArgumentNullException("baseStream");

            m_BaseStream = baseStream;
        }

        public sealed override bool CanRead
        {
            get { return m_BaseStream.CanRead; }
        }

        public sealed override bool CanSeek
        {
            get { return m_BaseStream.CanSeek; }
        }

        public sealed override bool CanWrite
        {
            get { return m_BaseStream.CanWrite; }
        }

        public sealed override void Flush()
        {
            m_BaseStream.Flush();
        }

        public sealed override long Length
        {
            get { return m_BaseStream.Length; }
        }

        public sealed override long Position
        {
            get
            {
                return m_BaseStream.Position;
            }
            set
            {
                m_BaseStream.Position = value;
            }
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            /* Copy a block of data that isn't swapped */
            Byte[] innerBuffer = new Byte[count];

            try
            {
                m_BaseStream.Read(innerBuffer, 0, count);
            }
            catch (Exception e)
            {
                throw e;
            }

            /* TODO: Some error checkign when larger byte swappers crashes on small buffers */

            /* Read into the new buffer swapped */
            for (int i = offset; i < count; i++)
            {
                buffer[i] = innerBuffer[(Int32)ComputeNextSwapPosition(i - offset)];
            }

            return count;
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            return m_BaseStream.Seek(offset, origin);
        }

        public sealed override void SetLength(long value)
        {
            m_BaseStream.SetLength(value);
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            Byte[] innerBuffer = new Byte[count];

            /* Write the data to inner buffer as unswapped */
            for (int i = offset; i < count; i++)
            {
                innerBuffer[(Int32)ComputeNextSwapPosition(i - offset)] = buffer[i];
            }

            try
            {
                m_BaseStream.Write(innerBuffer, 0, count);
            }
            catch { throw; }
        }

        public sealed override int ReadByte()
        {
            return m_BaseStream.ReadByte();
        }

        public sealed override void WriteByte(byte value)
        {
            m_BaseStream.WriteByte(value);
        }

        protected abstract Int64 ComputeNextSwapPosition(Int64 position);
    }

    public sealed class Int16SwapStream : ByteSwapStream
    {
        public Int16SwapStream(Stream baseStream)
            : base(baseStream)
        {
        }

        protected override long ComputeNextSwapPosition(long position)
        {
            return (position & unchecked(0x7FFFFFFFFFFFFFFEL)) + 1 - (position % 2);
        }
    }

    public sealed class Int32SwapStream : ByteSwapStream
    {
        public Int32SwapStream(Stream baseStream)
            : base(baseStream)
        {
        }

        protected override long ComputeNextSwapPosition(long position)
        {
            return (position & unchecked(0x7FFFFFFFFFFFFFFCL)) + 3 - (position % 4);
        }
    }

    public sealed class Int64SwapStream : ByteSwapStream
    {
        public Int64SwapStream(Stream baseStream)
            : base(baseStream)
        {
        }

        protected override long ComputeNextSwapPosition(long position)
        {
            return (position & unchecked(0x7FFFFFFFFFFFFFF8L)) + 7 - (position % 8);
        }
    }

    public sealed class Int96SwapStream : ByteSwapStream
    {
        public Int96SwapStream(Stream baseStream)
            : base(baseStream)
        {
        }

        protected override long ComputeNextSwapPosition(long position)
        {
            return ((position / 12) * 12) + 11 - (position % 12);
        }
    }
}