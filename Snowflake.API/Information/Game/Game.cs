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
        public Dictionary<string, string> Metadata { get; set; }
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public EmulatorSettings Settings { get; set; }
        public Game(string platformId, string name, GameImages images, Dictionary<string,string> metadata, string uuid, string fileName, EmulatorSettings settings=null)
        {
            this.PlatformId = platformId;
            this.Name = name;
            this.Images = images;
            this.Metadata = metadata;
            this.UUID = uuid;
            this.FileName = fileName;
            if (settings == null) settings = new EmulatorSettings();
            this.Settings = settings;
        }
    }
}
