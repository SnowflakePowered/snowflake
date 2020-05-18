/*
MIT License

Copyright (c) 2019 Bryan Perris

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.IO;

namespace Snowflake.Stone.FileSignatures.Formats.N64
{
    internal abstract class ByteSwapStream : Stream
    {
        private Stream m_BaseStream;

        protected ByteSwapStream(Stream baseStream)
        {
            if (baseStream == null)
                throw new ArgumentNullException(nameof(baseStream));

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
                throw new ArgumentNullException(nameof(buffer));

            /* Copy a block of data that isn't swapped */
            Byte[] innerBuffer = new Byte[count];

            try
            {
                m_BaseStream.Read(innerBuffer, 0, count);
            }
            catch (Exception)
            {
                throw;
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

        protected abstract long ComputeNextSwapPosition(long position);
    }
}
