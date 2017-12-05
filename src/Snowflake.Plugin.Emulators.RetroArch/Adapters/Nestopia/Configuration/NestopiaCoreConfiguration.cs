using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

using Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Selections;

namespace Snowflake.Plugin.Emulators.RetroArch.Adapters.Nestopia.Configuration
{
    [ConfigurationSection("nestopia", "Nestopia")]
    public interface NestopiaCoreConfiguration : IConfigurationSection<NestopiaCoreConfiguration>
    {
        [ConfigurationOption("nestopia_blargg_ntsc_filter", BlargFilter.Disabled, DisplayName = "Blarg NTSC Filter", Simple = true)]
        BlargFilter BlargFilter { get; set; }
        [ConfigurationOption("nestopia_palette", ColorPalette.Consumer, DisplayName = "Color Palette", Simple = true)]
        ColorPalette ColorPalette { get; set; }
        [ConfigurationOption("nestopia_nospritelimit", false, DisplayName = "Remove 8 sprites-per-scanline limit", Simple = true)]
        bool NoSpriteLimit { get; set; }
        [ConfigurationOption("nestopia_fds_auto_insert", false, DisplayName = "Automatically insert first FDS disk on reload")]
        bool FdsAutoInsert { get; set; }
        [ConfigurationOption("nestopia_overscan_v", true, DisplayName = "Mask Vertical Overscan")]
        bool MaskVerticalOverscan { get; set; }
        [ConfigurationOption("nestopia_overscan_h", true, DisplayName = "Mask Horizontal Overscan")]
        bool MaskHorizontalOverscan { get; set; }
        [ConfigurationOption("nestopia_aspect", PixelAspectRatio.EightSeven, DisplayName ="Preferred Aspect Ratio")]
        PixelAspectRatio PixelAspectRation { get; set; }
        [ConfigurationOption("nestopia_favored_system", NestopiaSystem.Auto, DisplayName = "Emulated System")]
        NestopiaSystem NestopiaSystem { get; set; }
    }
}
