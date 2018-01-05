using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Scraping.FileSignatures;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    // .z64
    public sealed class Nintendo64BigEndianFileSignature : Nintendo64FileSignature<Stream>
    {
        public Nintendo64BigEndianFileSignature()
            : base(0x80371240, "application/x-romfile-n64-bigendian", s => s)
        {
        }
    }
}
