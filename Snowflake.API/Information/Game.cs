using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.API.Base.Emulator;
namespace Snowflake.API.Information
{
    public class Game: Info
    {
        public Game(string platformId, string name, Dictionary<string,string> images, Dictionary<string,string> metadata, string uuid, string fileName, EmulatorSettings settings=null): base(platformId, name, images, metadata)
        {
            this.UUID = uuid;
            this.FileName = fileName;
            if (settings == null) settings = new EmulatorSettings();
            this.Settings = settings;
        }
        public string UUID { get; private set; }
        public string FileName { get; private set; }
        public EmulatorSettings Settings { get; set; }
    }
}
