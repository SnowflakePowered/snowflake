using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Information
{
    public class Platform : Info
    {
        public Platform(string platformId, string name, Dictionary<string, string> images, Dictionary<string, string> metadata, List<string> fileExtensions, string romIdentifierPlugin): base(platformId, name, images, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.RomIdentifierPlugin = romIdentifierPlugin;
        }
        public List<string> FileExtensions { get; private set; }
        public string RomIdentifierPlugin { get; private set; }

    }
}
