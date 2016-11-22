using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile.FileSignatures.Formats.CDXA
{
    internal class CDXABlockStream : Stream
    {
        private readonly Stream diskStream;
        public const long BlockLength = CDXADisk.BlockSize - CDXADisk.BlockHeaderSize;
        public CDXABlockStream(int lba, Stream diskStream)
        {
            this.diskStream = diskStream;
            this.LBA = lba;
            this.Length = CDXABlockStream.BlockLength;
        }
        public sealed override void Flush()
        {
            throw new NotImplementedException();
        }

        public sealed override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.Position = offset;
                    break;
                case SeekOrigin.End:
                    this.Position = this.Length + offset;
                    break;
                case SeekOrigin.Current:
                    this.Position += offset;
                    break;
            }
            return this.Position;
        }

        public sealed override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            this.diskStream.Seek((CDXADisk.BlockSize * this.LBA) + CDXADisk.BlockHeaderSize + this.Position, SeekOrigin.Begin);
            this.Position += count;
            return this.diskStream.Read(buffer, offset, count);
        }

        public sealed override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public sealed override bool CanRead => true;
        public sealed override bool CanSeek => true;
        public sealed override bool CanWrite => false;
        public sealed override long Length { get; }
        public int LBA { get; }
        public sealed override long Position { get; set; }
    }
}
