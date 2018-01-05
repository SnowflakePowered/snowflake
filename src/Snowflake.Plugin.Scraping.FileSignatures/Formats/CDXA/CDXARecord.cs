using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.CDXA
{
    internal class CDXARecord
    {
        public int LBA { get; }
        public long Length { get; }
        public string Path { get; }

        public CDXARecord(int lba, long length, string path)
        {
            this.LBA = lba;
            this.Length = length;
            this.Path = path;
        }
    }
}
