using System.Collections.Generic;
using Snowflake.Information;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public class GameInfo : Info, IGameInfo
    {
        public string UUID { get; }
        public string FileName { get; }
        public string CRC32 { get; }

        public GameInfo(string platformId, string name, IDictionary<string, string> metadata, string uuid, string fileName, string crc32)
            : base(platformId, name, metadata)
        {

            this.UUID = uuid;
            this.FileName = fileName;
            this.CRC32 = crc32;
        }
        public GameInfo(string platformId, string name, IDictionary<string, string> metadata, string uuid, string fileName)
            : this(platformId, name, metadata, uuid, fileName, FileHash.GetCRC32(fileName)) { }

        public static IGameInfo FromJson(dynamic json)
        {
            var metadata = json.Metadata.ToObject<IDictionary<string, string>>();
            string platformId = json.PlatformID;
            string name = json.Name;
            string uuid = json.UUID;
            string fileName = json.FileName;
            string crc32 = json.CRC32;
            return new GameInfo(platformId, name, metadata, uuid, fileName, crc32);
        }
    }
}
