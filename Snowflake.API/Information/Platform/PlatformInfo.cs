using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Information.Platform
{
    public class PlatformInfo : Info
    {
        public PlatformInfo(string platformId, string name, IDictionary<string, string> images, IDictionary<string, string> metadata, IList<string> fileExtensions, PlatformDefaults platformDefaults): base(platformId, name, images, metadata)
        {
            this.FileExtensions = fileExtensions;
            this.Defaults = platformDefaults;
        }
        public IList<string> FileExtensions { get; private set; }
        public PlatformDefaults Defaults { get; set; }

    }
}
