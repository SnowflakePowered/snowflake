using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;

using Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Nestopia.Selections;

namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Adapters.Nestopia.Configuration
{
    public class NestopiaCoreConfiguration : ConfigurationSection
    {
        [ConfigurationOption("nestopia_blargg_ntsc_filter", DisplayName = "Blarg NTSC Filter", Simple = true)]
        public BlargFilter BlargFilter { get; set; } = BlargFilter.Disabled;
        [ConfigurationOption("nestopia_palette", DisplayName = "Color Palette", Simple = true)]
        public ColorPalette ColorPalette { get; set; } = ColorPalette.Consumer;
        [ConfigurationOption("nestopia_nospritelimit", DisplayName = "Remove 8 sprites-per-scanline limit", Simple = true)]
        public bool NoSpriteLimit { get; set; } = false;
        [ConfigurationOption("nestopia_fds_auto_insert", DisplayName = "Automatically insert first FDS disk on reload")]
        public bool FdsAutoInsert { get; set; } = false;
        [ConfigurationOption("nestopia_overscan_v", DisplayName = "Mask Vertical Overscan")]
        public bool MaskVerticalOverscan { get; set; } = true;
        [ConfigurationOption("nestopia_overscan_h", DisplayName = "Mask Horizontal Overscan")]
        public bool MaskHorizontalOverscan { get; set; } = true;
        [ConfigurationOption("nestopia_aspect", DisplayName ="Preferred Aspect Ratio")]
        public PixelAspectRatio PixelAspectRation { get; set; } = PixelAspectRatio.EightSeven;
        [ConfigurationOption("nestopia_favored_system", DisplayName = "Emulated System")]
        public NestopiaSystem NestopiaSystem { get; set; } = NestopiaSystem.Auto;
        public NestopiaCoreConfiguration() : base("nestopia", "Nestopia Core Options", "Core options for the Nestopia core")
        {
        }
    }
}
