using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snowflake.Stone.FileSignatures.Formats.CDI.DiscJugglerDisc;

namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    internal class DiscJugglerBlockStream : Stream
    {
        private readonly Stream diskStream;

        internal DiscJugglerBlockStream(uint lba, Track track, Stream diskStream)
        {
            this.diskStream = diskStream;
            this.LBA = lba;
            this.Track = track;
            this.Length = this.Track.DataSize;
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

        /// <inheritdoc/>
        public sealed override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Does not protect against out of bounds reading.
        /// To access file contents, use the much safer <see cref="DiscJugglerDisc.OpenFile"/>
        /// </remarks>
        public sealed override int Read(byte[] buffer, int offset, int count)
        {
            //long offset = track.file_offset + frameAddr * track.SectorSize;
            //using var reader = new BinaryReader(this.ImageStream, Encoding.UTF8, true);
            //this.ImageStream.Seek(offset, SeekOrigin.Begin);
            //this.ImageStream.Seek(track.HeaderSize, SeekOrigin.Current);
            //return reader.ReadBytes(track.DataSize);

            long baseoffset = this.Track.Position + ((this.Track.BeginFrameAddr + this.LBA) * this.Track.SectorSize);

            this.diskStream.Seek(baseoffset, SeekOrigin.Begin);
            this.diskStream.Seek(this.Track.HeaderSize, SeekOrigin.Current);
            this.diskStream.Seek(this.Position, SeekOrigin.Current);
            
            this.Position += count;
            return this.diskStream.Read(buffer, offset, count);
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

        public uint LBA { get; }
        private Track Track { get; }

        /// <inheritdoc/>
        public sealed override long Position { get; set; }
    }
}
