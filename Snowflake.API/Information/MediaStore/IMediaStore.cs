using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Information.MediaStore
{
    public interface IMediaStore
    {
        IMediaStoreSection Images { get; }
        IMediaStoreSection Audio { get; }
        IMediaStoreSection Video { get; }
        IMediaStoreSection Resources { get; }
        string MediaStoreKey { get; }

    }
}
