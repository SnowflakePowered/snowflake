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
        /// <summary>
        /// This method is used as part of a core function, 
        /// and does not serve the pure data role of the PlatformInfo class.
        /// Extension methods are put in when they are unrelated to the class and is not used as part of the class.
        /// </summary>
        /// <param name="platform"></param>
        /// <returns></returns>
        public static ScrapeEngine GetScrapeEngine(this PlatformInfo platform) 
        {
            return new ScrapeEngine(platform);
        }
    }
}
