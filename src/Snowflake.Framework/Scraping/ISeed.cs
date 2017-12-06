using System;

namespace Snowflake.Scraping
{
    public interface ISeed
    {
        Guid Guid { get; }
        Guid Parent { get; }
        string Type { get; }
        string Value { get; }
        string Source { get; }
        bool IsCulled { get; }
        void Cull();
    }
}