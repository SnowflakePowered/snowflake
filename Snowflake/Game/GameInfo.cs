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
        public string MediaStoreKey { get; private set; }
        public IMediaStore MediaStore { get { return new FileMediaStore(this.MediaStoreKey); } }
        public GameInfo(string platformId, string name, IDictionary<string, string> metadata, string uuid, string fileName, string crc32)
            : base(platformId, name, metadata)
        {
            this.MediaStoreKey = this.Metadata["kori_mediastorekey"];
            this.UUID = uuid;
            this.FileName = fileName;
            this.CRC32 = crc32;
        }
        public GameInfo(string platformId, string name, IMediaStore mediaStore, IDictionary<string, string> metadata, string uuid, string fileName)
            : this(platformId, name, mediaStore, metadata, uuid, fileName, FileHash.GetCRC32(fileName)) { }

        public static IGameInfo FromJson(dynamic json)
        {
            var metadata = json.Metadata.ToObject<IDictionary<string, string>>();
            string mediaStoreKey = json.MediaStore.MediaStoreKey;
            var mediastore = new FileMediaStore(mediaStoreKey);
            string platformId = json.PlatformID;
            string name = json.Name;
            string uuid = json.UUID;
            string fileName = json.FileName;
            string crc32 = json.CRC32;
            return new GameInfo(platformId, name, mediastore, metadata, uuid, fileName, crc32);
        }
    }
}
