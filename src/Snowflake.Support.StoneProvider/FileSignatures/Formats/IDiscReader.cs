﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Snowflake.Stone.FileSignatures.Formats
{
    public interface IDiscReader
    {
        Stream OpenBlock(int lba);
        string VolumeDescriptor { get; }
    }
}