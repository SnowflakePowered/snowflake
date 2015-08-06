namespace Snowflake.Platform
{
    public class PlatformDefaults : IPlatformDefaults
    {
        public string Scraper { get; set; }
        public string Emulator { get; set; }

        public PlatformDefaults(string scraper, string emulator)
        {
            this.Scraper = scraper;
            this.Emulator = emulator;
        }
    }
}
