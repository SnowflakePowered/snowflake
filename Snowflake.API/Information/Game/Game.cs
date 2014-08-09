using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Information.Game
{
    public class Game: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public GameImages Images { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public IDictionary<string, dynamic> Settings { get; set; }
        public Game(string platformId, string name, GameImages images, IDictionary<string,string> metadata, string uuid, string fileName, IDictionary<string, dynamic> settings)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.Images = images;
            this.Metadata = metadata;
            this.UUID = uuid;
            this.FileName = fileName;
            this.Settings = settings;
        }
    }
}
