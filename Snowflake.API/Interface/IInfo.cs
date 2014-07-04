using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Interface
{
    public interface IInfo
    {
        string PlatformId { get; }
        string Name { get; }
        Dictionary<string, string> Images { get; }
        Dictionary<string, string> Metadata { get; set; }

    }
}
