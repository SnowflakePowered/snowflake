using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Stone.FileSignatures;

namespace Snowflake.Stone.FileSignatures.Nintendo
{
    // .z64
    internal sealed class Nintendo64BigEndianFileSignature : Nintendo64FileSignature<Stream>
    {
        public Nintendo64BigEndianFileSignature()
            : base(0x80371240, s => s)
        {
        }
    }
}
