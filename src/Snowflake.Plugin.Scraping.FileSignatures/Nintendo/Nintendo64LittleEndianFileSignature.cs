using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Plugin.Scraping.FileSignatures;
using Snowflake.Plugin.Scraping.FileSignatures.Formats.N64;

namespace Snowflake.Plugin.Scraping.FileSignatures.Nintendo
{
    // .n64
    public class Nintendo64LittleEndianFileSignature : Nintendo64FileSignature<Int32SwapStream>
    {
        public Nintendo64LittleEndianFileSignature()
            : base(0x40123780, s => new Int32SwapStream(s))
        {
        }
    }
}
