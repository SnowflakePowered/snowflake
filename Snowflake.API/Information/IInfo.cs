using System.Collections.Generic;

namespace Snowflake.Information
{
    public interface IInfo
    {
        string PlatformId { get; }
        string Name { get; }
        IDictionary<string, string> Metadata { get; set; }

    }
}
