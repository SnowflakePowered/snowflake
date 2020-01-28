﻿using System;
using System.Collections.Generic;
using Snowflake.Filesystem;

namespace Snowflake.Orchestration.Saving
{
    public interface ISaveGame
    {
        DateTimeOffset CreatedTimestamp { get; }
        Guid Guid { get; }
        IReadOnlyDirectory SaveContents { get; }
        string SaveType { get; }
        IEnumerable<string> Tags { get; }
    }
}