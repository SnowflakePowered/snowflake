using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Plugin.Scraping.FileSignatures.Formats.CDXA
{
    public class CDXAFile
    {
        private readonly Stream diskStream;
        public int Lba { get; }
        public string Path { get; }
        public long Length { get; }
        public CDXAFile(Stream diskStream, int lba, string path, long length)
        {
            this.diskStream = diskStream;
            this.Lba = lba;
            this.Path = path;
            this.Length = length;
        }
    }
}
