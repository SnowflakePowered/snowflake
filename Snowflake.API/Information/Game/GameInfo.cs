using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Information.MediaStore;

namespace Snowflake.Information.Game
{
    public class GameInfo: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public IMediaStore MediaStore { get; private set; }
        public IDictionary<string, string> Metadata { get; set; }
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public IDictionary<string, dynamic> Settings { get; set; }
        public GameInfo(string platformId, string name, IMediaStore mediaStore, IDictionary<string,string> metadata, string uuid, string fileName, IDictionary<string, dynamic> settings)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.MediaStore = mediaStore;
            this.Metadata = metadata;
            this.UUID = uuid;
            this.FileName = fileName;
            this.Settings = settings;
        }
    }
}
