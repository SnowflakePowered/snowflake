using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.MediaStore;
using Snowflake.Information;

namespace Snowflake.Game
{
    public class GameInfo: Info
    {
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public IDictionary<string, dynamic> Settings { get; set; }
        public GameInfo(string platformId, string name, IMediaStore mediaStore, IDictionary<string,string> metadata, string uuid, string fileName, IDictionary<string, dynamic> settings) : base(platformId,name,mediaStore,metadata)
        {
            this.UUID = uuid;
            this.FileName = fileName;
            this.Settings = settings;
        }
    }
}
