using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;

namespace Snowflake.Tests.Fakes
{
    [Obsolete]
    internal class FakeMediaStore : IMediaStore
    {
        public IMediaStoreSection Resources { get; set; }
        public IMediaStoreSection Video { get; set; }
        public IMediaStoreSection Audio { get; set; }
        public IMediaStoreSection Images { get; set; }
        public string MediaStoreKey { get; set; }
    }
}
