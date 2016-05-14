using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using Snowflake.Records;
using Snowflake.Records.File;
using Snowflake.Records.Game;
using Snowflake.Records.Metadata;
using Snowflake.Utility;
namespace Snowflake.Game
{
    [Obsolete("Will be replaced with Snowflake.Records.GameRecord")]
    public partial class GameInfo : IGameInfo
    {
        private static readonly Guid ObsoleteGameInfoNamespace = new Guid("33f6e774-fbbc-4a7f-902a-446d1deba390");
        public string UUID => this.Metadata["obsolete_uuid"];
        public string FileName => this.Metadata["obsolete_filename"];
        public string CRC32 => this.Metadata["obsolete_crc32"];
        public string PlatformID => this.PlatformId;
        public string PlatformId => this.Metadata[GameMetadataKeys.Platform];
        public string Title => this.Metadata[GameMetadataKeys.Title];

        public IList<IFileRecord> Files
            => new List<IFileRecord> {new FileRecord(this.FileName, "application/x-romfile-unknown", this)};

        IDictionary<string, string> IGameInfo.Metadata
        {
            get
            {
                return this.Metadata.ToDictionary(m => m.Key, m => m.Value.Value);
            }
            set
            {
                this.Metadata = new MetadataCollection(this.Guid, value.ToDictionary(m => m.Key,
                    m => new RecordMetadata(m.Key, m.Value, this.Guid) as IRecordMetadata));
            }
        }
        public IMetadataCollection Metadata { get; private set; }
        public Guid Guid { get; }

        internal GameInfo(IGameRecord record)
        {
            this.Guid = record.Guid;
            this.Metadata = record.Metadata;
        }

        public GameInfo(string uuid, string platformName, string fileName, string title, string crc32, IDictionary<string, string> metadata)
        {
            this.Guid = GuidUtility.Create(GameInfo.ObsoleteGameInfoNamespace, uuid);
            (this as IGameInfo).Metadata = metadata;
            this.Metadata.Add("obsolete_uuid", new RecordMetadata("obsolete_uuid", uuid, this.Guid));
            this.Metadata.Add(GameMetadataKeys.Platform,
                new RecordMetadata(GameMetadataKeys.Platform, platformName, this.Guid));
            this.Metadata.Add(GameMetadataKeys.Title,
                new RecordMetadata(GameMetadataKeys.Title, title, this.Guid));
            this.Metadata.Add("obsolete_filename",
                new RecordMetadata("obsolete_filename", fileName, this.Guid));
            this.Metadata.Add("obsolete_crc32",
                new RecordMetadata("obsolete_crc32", crc32, this.Guid));
        }

        public GameInfo(string platformId, string name, string fileName, IDictionary<string, string> metadata)
    : this(GameInfo.GetUUID(fileName, platformId), platformId, fileName, name, FileHash.GetCRC32(fileName), metadata) { }

        public static string GetUUID(string fileName, string platformId)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            byte[] hashBuffer = new byte[2 * 1024];
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
