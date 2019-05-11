using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Stone.FileSignatures.Formats.CDXA
{
    internal class CDXARecord
    {
        public uint LBAStart { get; }
        public long Length { get; }
        public string Path { get; }

        public CDXARecord(uint lba, long length, string path)
        {
            this.LBAStart = lba;
            this.Length = length;
            this.Path = path;
        }
    }
}
