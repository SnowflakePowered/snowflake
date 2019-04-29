using System;
using System.Collections.Generic;
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
            string meta = Encoding.UTF8.GetString(this.Disc.ReadSectors(this.Disc.Sessions[1], 1));
            return meta.Trim('\0').Trim();
        }

    }
}
