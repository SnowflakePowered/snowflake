using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base.Emulator;
using Snowflake.API.Interface;
namespace Snowflake.API.Information.Game
{
    public class Game: IInfo
    {
        public string PlatformId { get; private set; }
        public string Name { get; private set; }
        public GameImages Images { get; set; }
        public Dictionary<string, dynamic> Metadata { get; set; }
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public Dictionary<string, dynamic> Settings { get; set; }
        public Game(string platformId, string name, GameImages images, Dictionary<string, dynamic> metadata, string uuid, string fileName, Dictionary<string, dynamic> settings)
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
