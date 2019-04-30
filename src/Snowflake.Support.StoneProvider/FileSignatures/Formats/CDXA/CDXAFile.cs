using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    public class CDXAFile
    {
        private readonly Stream diskStream;
        public uint LBAStart { get; }
        public string Path { get; }
        public long Length { get; }

        public CDXAFile(Stream diskStream, uint lba, string path, long length)
        {
            this.diskStream = diskStream;
            this.LBAStart = lba;
            this.Path = path;
            this.Length = length;
        }

        public CDXAFileStream OpenFile()
        {
            return new CDXAFileStream(this.LBAStart, this.Length, this.diskStream);
        }
    }
}
