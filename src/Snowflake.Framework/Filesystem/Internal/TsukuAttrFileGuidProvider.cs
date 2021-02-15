using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zio;
using Tsuku.Extensions;

namespace Snowflake.Filesystem.Internal
{
    internal sealed class TsukuAttrFileGuidProvider : IFileGuidProvider
    {
        public static readonly string SnowflakeFile = "Snowflake.File";

        public static IFileGuidProvider TsukuGuidProvider = new TsukuAttrFileGuidProvider();

        public bool TryGetGuid(FileInfo rawInfo, out Guid guid)
        {
            return rawInfo.TryGetGuidAttribute(SnowflakeFile, out guid);
        }

        public void SetGuid(FileInfo rawInfo, Guid guid)
        {
            rawInfo.SetAttribute(SnowflakeFile, guid);
        }
    }
}
