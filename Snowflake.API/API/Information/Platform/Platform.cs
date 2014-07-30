using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.API.Information.Platform
{
    public class Platform : Info
    {
        public Platform(string platformId, string name, IDictionary<string, string> images, IDictionary<string, string> metadata, IList<string> fileExtensions, PlatformDefaults platformDefaults): base(platformId, name, images, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
        }
        public IList<string> FileExtensions { get; private set; }
        public PlatformDefaults Defaults { get; set; }

    }
}
