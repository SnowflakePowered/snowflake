using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Filesystem
{
    public class InMemoryFileGuidProvider : IFileGuidProvider
    {
        public static InMemoryFileGuidProvider GuidProvider { get; } = new();
        private ConcurrentDictionary<FileInfo, Guid> GuidStore { get; } = new ConcurrentDictionary<FileInfo, Guid>();

        public void SetGuid(FileInfo rawInfo, Guid guid)
        {
            this.GuidStore.TryAdd(rawInfo, guid);
        }

        public bool TryGetGuid(FileInfo rawInfo, out Guid guid)
        {
            return this.GuidStore.TryGetValue(rawInfo, out guid);
        }
    }
}
