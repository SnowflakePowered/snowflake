using System;

namespace Snowflake.Scraping
{
    public interface ISeed
    {
        Guid Guid { get; }
        Guid Parent { get; }
        SeedContent Content { get; }
        string Source { get; }
        bool IsCulled { get; }
        void Cull();
    }
}