using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    public class CDXAFileStream : Stream
    {
        private readonly Stream diskStream;
        public const int BlockLength = CDXADisk.BlockSize - CDXADisk.BlockHeaderSize;
        public const int DataBlockLength = 0x800;

        public CDXAFileStream(int lbaStart, long fileLength, Stream diskStream)
        {
            this.diskStream = diskStream;
            this.LBA = lbaStart;
            this.Length = fileLength;
        }

        /// <inheritdoc/>
        public sealed override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset > this.Length)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    this.Position = offset;
                    break;
                case SeekOrigin.End:
                    if (this.Length + offset > this.Length)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    this.Position = this.Length + offset;
                    break;
                case SeekOrigin.Current:
                    if (this.Position + offset > this.Length)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    this.Position += offset;
                    break;
            }

            return this.Position;
        }

        /// <inheritdoc/>
        public sealed override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            int bytesLeftToRead = (int) Math.Min(this.Length - this.Position, count);
            long fullBlocksToSeek = this.Position / CDXAFileStream.DataBlockLength;
            long remainderBytesToSeek = this.Position % CDXAFileStream.DataBlockLength;
            int bytesUntilNextLba = CDXAFileStream.DataBlockLength;
            int totalBytesRead = 0;
            int byteArrayOffset = offset;

            this.diskStream.Seek((CDXADisk.BlockSize * (this.LBA + fullBlocksToSeek)),
                SeekOrigin.Begin); // Seek to the beginning of the file.
            // now at the beginning of a new LBA.

            this.diskStream.Seek(CDXADisk.BlockHeaderSize,
                SeekOrigin.Current); // Seek past the header of the first LBA.
            if (remainderBytesToSeek != 0) // there are still bytes to seek the the position.
            {
                this.diskStream.Seek(remainderBytesToSeek, SeekOrigin.Current); // seek to the bytes.
                bytesUntilNextLba =
                    CDXAFileStream.DataBlockLength -
                    (int) remainderBytesToSeek; //bytes until we have to skip to the next LBA.
            }

            while (bytesLeftToRead > 0)
            {
                int bytesRead =
                    this.diskStream.Read(buffer, byteArrayOffset, Math.Min(bytesUntilNextLba, bytesLeftToRead));
                this.diskStream.Seek(CDXAFileStream.BlockLength - CDXAFileStream.DataBlockLength,
                    SeekOrigin.Current); // now at a new LBA
                this.diskStream.Seek(CDXADisk.BlockHeaderSize, SeekOrigin.Current); // Seek past the next header. 
                bytesLeftToRead -= bytesRead;
                totalBytesRead += bytesRead;
                byteArrayOffset += bytesRead;
                this.Position += bytesRead;
            }

            return totalBytesRead;
        }

        /// <inheritdoc/>
        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public sealed override bool CanRead => true;

        /// <inheritdoc/>
        public sealed override bool CanSeek => true;

        /// <inheritdoc/>
        public sealed override bool CanWrite => false;

        /// <inheritdoc/>
        public sealed override long Length { get; }

        public int LBA { get; }

        /// <inheritdoc/>
        public sealed override long Position { get; set; }
    }
}
