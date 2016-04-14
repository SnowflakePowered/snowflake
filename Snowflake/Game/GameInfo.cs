using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Snowflake.Information;
using Snowflake.Utility;

namespace Snowflake.Game
{
    public partial class GameInfo :  IGameInfo
    {
        public string UUID { get; }
        public string FileName { get; }
        public string CRC32 { get; }
        public string PlatformID { get; }
        public string Title { get; }
        public IDictionary<string, string> Metadata { get; set; }
        public GameInfo(string uuid, string platformName, string fileName, string title, string crc32, IDictionary<string, string> metadata)
        {
            this.PlatformID = platformName;
            this.Title = title;
            this.Metadata = metadata;
            this.UUID = uuid;
            this.FileName = fileName;
            this.CRC32 = crc32;
        }
        public GameInfo(string platformId, string name, string fileName, IDictionary<string, string> metadata)
            : this(GameInfo.GetUUID(fileName, platformId), platformId, fileName, name, FileHash.GetCRC32(fileName), metadata) { }

        public static string GetUUID(string fileName, string platformId)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] hashBuffer = new byte[2*1024];
            using (FileStream romFile = File.OpenRead(fileName))
            {
                romFile.Read(hashBuffer, 0, hashBuffer.Length); //read the first two kilobytes of the rom
            }
            //todo use new algo for unique ids.
            return $"snowflakehash-{BitConverter.ToString(sha1.ComputeHash(hashBuffer)).Replace("-", string.Empty).ToLowerInvariant()}-{platformId}";
        }

        public static IGameInfo FromJson(dynamic json)
        {
            IDictionary<string, string> metadata = json.Metadata.ToObject<IDictionary<string, string>>();
            string platformId = json.PlatformID;
            string name = json.Name;
            string uuid = json.UUID;
            string fileName = json.FileName;
            string crc32 = json.CRC32;
            return new GameInfo(uuid, platformId, name, fileName, crc32, metadata);
        }
    }
}
