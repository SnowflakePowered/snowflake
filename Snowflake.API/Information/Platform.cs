using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Information.Game
{
    public class Platform : Info
    {
        public Platform(string platformId, string name, Dictionary<string, string> images, Dictionary<string, dynamic> metadata, List<string> fileExtensions): base(platformId, name, images, metadata)
        {
            this.FileExtensions = fileExtensions;
        }
        public List<string> FileExtensions { get; private set; }
    }
}
