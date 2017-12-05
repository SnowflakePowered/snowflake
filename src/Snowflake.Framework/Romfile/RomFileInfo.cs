using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Romfile
{
    public class RomFileInfo : IRomFileInfo
    {
        /// <inheritdoc/>
        public string Mimetype { get; }

        /// <inheritdoc/>
        public string Serial { get; }

        /// <inheritdoc/>
        public string InternalName { get; }

        public RomFileInfo(string mimetype, string serial, string internalName)
        {
            this.Mimetype = mimetype;
            this.Serial = serial;
            this.InternalName = internalName;
        }
    }
}
