using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    /// <summary>
    /// Represents a Dreamcast Disc from a DiscJuggler CDI Image
    /// </summary>
    public class CdiDreamcastDisc
    {
        public CdiDreamcastDisc(DiscJugglerDisc cdiDisc)
        {
            Disc = cdiDisc;
        }

        private DiscJugglerDisc Disc { get; }
        public string GetMeta()
        {
            byte[] array = new byte[0xff];
            this.Disc.OpenBlock(0).Read(array, 0, 0xff);
            string meta = Encoding.UTF8.GetString(array);
            return meta.Trim('\0');
        }

    }
}
