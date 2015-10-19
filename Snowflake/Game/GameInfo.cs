using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
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
        public GameInfo(string platformId, string name, IDictionary<string, string> metadata, string fileName)
            : this(platformId, name, metadata, GameInfo.GetUUID(fileName, platformId), fileName, FileHash.GetCRC32(fileName)) { }


        public static string GetUUID(string fileName, string platformId)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] hashBuffer = new byte[2*1024];
            using (FileStream romFile = File.OpenRead(fileName))
            {
                romFile.Read(hashBuffer, 0, hashBuffer.Length); //read the first two kilobytes of the rom
            }
            
            return $"snowflakehash-{BitConverter.ToString(sha1.ComputeHash(hashBuffer)).Replace("-", string.Empty).ToLowerInvariant()}-{platformId}";
    }
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
