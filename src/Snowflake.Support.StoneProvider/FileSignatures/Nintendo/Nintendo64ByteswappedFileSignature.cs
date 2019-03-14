using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Stone.FileSignatures;
using Snowflake.Stone.FileSignatures.Formats.N64;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    // .v64
    public class Nintendo64ByteswappedFileSignature : Nintendo64FileSignature<Int16SwapStream>
    {
        public Nintendo64ByteswappedFileSignature()
            : base(0x37804012, s => new Int16SwapStream(s))
        {
        }
    }
}
