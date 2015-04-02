using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;
using Snowflake.Information;
using Snowflake.Utility;
using System.IO;
using Newtonsoft.Json;
namespace Snowflake.Game
{
    public class GameInfo : Info, IGameInfo
    {
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public string CRC32 { get; private set; }
        [JsonIgnore]
        public IMediaStore MediaStore { get { return new FileMediaStore(this.Metadata["snowflake_mediastorekey"]); } }
        public GameInfo(string platformId, string name, IDictionary<string, string> metadata, string uuid, string fileName, string crc32)
            : base(platformId, name, metadata)
        {
            if(!this.Metadata.ContainsKey("snowflake_mediastorekey")){
                this.Metadata.Add("snowflake_mediastorekey", GameInfo.ValidateFilename(metadata["game_title"]).Replace(' ', '_') + uuid);
            }
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
        private static string ValidateFilename(string text, char? replacement = '_')
        {
            //from http://stackoverflow.com/a/25223884/1822679
            StringBuilder sb = new StringBuilder(text.Length);
            var invalids = Path.GetInvalidFileNameChars();
            bool changed = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (invalids.Contains(c))
                {
                    changed = true;
                    var repl = replacement ?? '\0';
                    if (repl != '\0')
                        sb.Append(repl);
                }
                else
                    sb.Append(c);
            }
            if (sb.Length == 0)
                return "_";
            return changed ? sb.ToString() : text;
        }
    }
}
