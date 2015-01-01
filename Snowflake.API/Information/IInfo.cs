using System.Collections.Generic;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information
{
    public interface IInfo
    {
        string PlatformId { get; }
        string Name { get; }
        IDictionary<string, string> Metadata { get; set; }
        IMediaStore MediaStore { get;  }

    }
}
