using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core;
using Snowflake.Information.Platform;

namespace Snowflake.Extensions
{
    public static class PlatformExts
    {
        public static ScrapeEngine GetScrapeEngine(this PlatformInfo platform)
        {
            return new ScrapeEngine(platform);
        }
    }
}
