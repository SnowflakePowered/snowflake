using System;
namespace Snowflake.Platform
{
    public interface IPlatformDefaults
    {
        string Emulator { get; set; }
        string Identifier { get; set; }
        string Scraper { get; set; }
    }
}
