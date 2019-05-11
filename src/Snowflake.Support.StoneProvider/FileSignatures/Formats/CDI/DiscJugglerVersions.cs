using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    internal enum DiscJugglerVersions : uint
    {
        /// <summary>
        /// Unknown version, use <see cref="DiscJugglerDisc.DiscJugglerRawVersion"/> to 
        /// determine version.
        /// </summary>
        UnknownVersion = 0,
        /// <summary>
        /// DiscJuggler version 2
        /// </summary>
        Version2 = 0x80000004,
        /// <summary>
        /// DiscJuggler version 3
        /// </summary>
        Version3 = 0x80000005,
        /// <summary>
        /// DiscJuggler version 3+
        /// </summary>
        Version35 = 0x80000006,
    }
}
