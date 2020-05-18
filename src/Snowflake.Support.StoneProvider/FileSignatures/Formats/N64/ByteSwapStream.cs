/*
cor64 - https://github.com/bryanperris/cor64
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
        private Stream baseStream;

        protected ByteSwapStream(Stream baseStream)
        {
            this.baseStream = baseStream;
        }

        public sealed override bool CanRead
        {
            get { return this.baseStream.CanRead; }
        }

        public sealed override bool CanSeek
        {
            get { return this.baseStream.CanSeek; }
        }

        public sealed override bool CanWrite
        {
            get { return this.baseStream.CanWrite; }
        }

        public sealed override void Flush()
        {
            this.baseStream.Flush();
        }

        public sealed override long Length
        {
            get { return this.baseStream.Length; }
        }

        public sealed override long Position
        {
            get
            {
                return this.baseStream.Position;
            }
            set
            {
                this.baseStream.Position = value;
            }
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            /* Copy a block of data that isn't swapped */
            byte[] innerBuffer = new byte[count];

            try
            {
                this.baseStream.Read(innerBuffer, 0, count);
            }
            catch (Exception)
            {
                throw;
            }

            /* TODO: Some error checkign when larger byte swappers crashes on small buffers */

            /* Read into the new buffer swapped */
            for (int i = offset; i < count; i++)
            {
                buffer[i] = innerBuffer[(int)ComputeNextSwapPosition(i - offset)];
            }

            return count;
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        public sealed override void SetLength(long value)
        {
            this.baseStream.SetLength(value);
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            byte[] innerBuffer = new byte[count];

            /* Write the data to inner buffer as unswapped */
            for (int i = offset; i < count; i++)
            {
                innerBuffer[(int)ComputeNextSwapPosition(i - offset)] = buffer[i];
            }

            this.baseStream.Write(innerBuffer, 0, count);
        }

        public sealed override int ReadByte()
        {
            return this.baseStream.ReadByte();
        }

        public sealed override void WriteByte(byte value)
        {
            this.baseStream.WriteByte(value);
        }

        protected abstract long ComputeNextSwapPosition(long position);
    }
}
