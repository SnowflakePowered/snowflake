using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats
{
    internal interface IDiscReader
    {
        Stream OpenBlock(uint lba);
        string VolumeDescriptor { get; }
    }
}
