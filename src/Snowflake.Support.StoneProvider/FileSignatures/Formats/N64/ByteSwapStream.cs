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

namespace Snowflake.Stone.FileSignatures.Formats.N64
{
    public abstract class ByteSwapStream : Stream
    {
        private Stream baseStream;

        protected ByteSwapStream(Stream baseStream)
        {
            if (baseStream == null)
            {
                throw new ArgumentNullException("baseStream");
            }

            this.baseStream = baseStream;
        }

        /// <inheritdoc/>
        public sealed override bool CanRead => this.baseStream.CanRead;

        /// <inheritdoc/>
        public sealed override bool CanSeek => this.baseStream.CanSeek;

        /// <inheritdoc/>
        public sealed override bool CanWrite => this.baseStream.CanWrite;

        /// <inheritdoc/>
        public sealed override void Flush()
        {
            this.baseStream.Flush();
        }

        /// <inheritdoc/>
        public sealed override long Length => this.baseStream.Length;

        /// <inheritdoc/>
        public sealed override long Position
        {
            get { return this.baseStream.Position; }
            set { this.baseStream.Position = value; }
        }

        /// <inheritdoc/>
        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

            /* Copy a block of data that isn't swapped */
            byte[] innerBuffer = new byte[count];

            try
            {
                this.baseStream.Read(innerBuffer, 0, count);
            }
            catch (Exception e)
            {
                throw e;
            }

            /* TODO: Some error checkign when larger byte swappers crashes on small buffers */

            /* Read into the new buffer swapped */
            for (int i = offset; i < count; i++)
            {
                buffer[i] = innerBuffer[(int) this.ComputeNextSwapPosition(i - offset)];
            }

            return count;
        }

        /// <inheritdoc/>
        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            return this.baseStream.Seek(offset, origin);
        }

        /// <inheritdoc/>
        public sealed override void SetLength(long value)
        {
            this.baseStream.SetLength(value);
        }

        /// <inheritdoc/>
        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            byte[] innerBuffer = new byte[count];

            /* Write the data to inner buffer as unswapped */
            for (int i = offset; i < count; i++)
            {
                innerBuffer[(int) this.ComputeNextSwapPosition(i - offset)] = buffer[i];
            }

            try
            {
                this.baseStream.Write(innerBuffer, 0, count);
            }
            catch
            {
                throw;
            }
        }

        /// <inheritdoc/>
        public sealed override int ReadByte()
        {
            return this.baseStream.ReadByte();
        }

        /// <inheritdoc/>
        public sealed override void WriteByte(byte value)
        {
            this.baseStream.WriteByte(value);
        }

        protected abstract long ComputeNextSwapPosition(long position);
    }
}
