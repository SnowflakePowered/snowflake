using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;
using Snowflake.Information;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public class GameInfo : Info, IGameInfo
    {
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public string CRC32 { get; private set; }
        public GameInfo(string platformId, string name, IMediaStore mediaStore, IDictionary<string, string> metadata, string uuid, string fileName, string crc32)
            : base(platformId, name, mediaStore, metadata)
        {
            this.UUID = uuid;
            this.FileName = fileName;
            this.CRC32 = crc32;
        }
        public GameInfo(string platformId, string name, IMediaStore mediaStore, IDictionary<string, string> metadata, string uuid, string fileName)
            : this(platformId, name, mediaStore, metadata, uuid, fileName, FileHash.GetCRC32(fileName)) { }

    }
}
