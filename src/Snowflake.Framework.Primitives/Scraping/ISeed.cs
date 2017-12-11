using System;
using Snowflake.Scraping.Extensibility;

namespace Snowflake.Scraping
{
    public interface ISeed
    {
        Guid Guid { get; }
        Guid Parent { get; }
        SeedContent Content { get; }
        string Source { get; }
    }
}