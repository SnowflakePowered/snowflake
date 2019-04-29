using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats.CDI
{
    /// <summary>
    /// Occurs when there was an error parsing the DiscJuggler CDI Image
    /// </summary>
    public sealed class DiscJugglerParseException : Exception
    {
        internal DiscJugglerParseException(string message) : base(message)
        {
        }
    }
}
