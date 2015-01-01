using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Platform
{
    public class PlatformDefaults : IPlatformDefaults
    {
        public string Scraper { get; set; }
        public string Identifier { get; set; }
        public string Emulator { get; set; }

        public PlatformDefaults(string scraper, string identifier, string emulator)
        {
            this.Scraper = scraper;
            this.Identifier = identifier;
            this.Emulator = emulator;
        }
    }
}
