using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Configuration
{
    /*
       Taken from a section of Dolphin.ini
       FullscreenResolution = Auto
       Fullscreen = True
       RenderToMain = True
       RenderWindowXPos = -1
       RenderWindowYPos = -1
       RenderWindowWidth = 640
       RenderWindowHeight = 480
       RenderWindowAutoSize = False
       KeepWindowOnTop = False
       ProgressiveScan = False
       PAL60 = False
       ISOPath0 = C:\Dumps\Wii\RMGE01.wbfs
    */

    [ConfigurationSection("Display", "Display")]
    public partial interface ExampleConfigurationSection
    {
        [ConfigurationOption("FullscreenResolution", FullscreenResolution.Auto)]
        FullscreenResolution FullscreenResolution { get; set; }

        [ConfigurationOption("Fullscreen", true)]
        bool Fullscreen { get; set; }

        [ConfigurationOption("RenderToMain", true)]
        bool RenderToMain { get; set; }

        [ConfigurationOption("RenderWindowWidth", 640)]
        int RenderWindowWidth { get; set; }

        [ConfigurationOption("RenderWindowHeight", 480)]
        int RenderWindowHeight { get; set; }

        [ConfigurationOption("ISOPath0", @"game:/program/RMGE01.wbfs", PathType.File)]
        string ISOPath0 { get; set; }

        [ConfigurationOption("ISODir", @"game:/program", PathType.Directory)]
        string ISODir { get; set; }

        [ConfigurationOption("InternalCpuRatio", 1.0)]
        double InternalCpuRatio { get; set; }

        [ConfigurationOption("application/vnd.snowflake-resource.dolphin-respack")]
        Guid SomeResource { get; set; }
    }

    public enum FullscreenResolution
    {
        [SelectionOption("Auto", DisplayName = "Auto")]
        Auto,
        [SelectionOption("640x480")] Resolution640X480,
        [SelectionOption("720x480")] Resolution720X480,
        [SelectionOption("720x576")] Resolution720X576,
        [SelectionOption("800x480")] Resolution800X480,
        [SelectionOption("800x600")] Resolution800X600,
        [SelectionOption("1024x600")] Resolution1024X600,
        [SelectionOption("1024x768")] Resolution1024X768,
        [SelectionOption("1152x648")] Resolution1152X648,
        [SelectionOption("1280x720")] Resolution1280X720,
        [SelectionOption("1280x768")] Resolution1280X768,
        [SelectionOption("1280x1024")] Resolution1280X1024,
        [SelectionOption("1360x768")] Resolution1360X768,
        [SelectionOption("1400x1050")] Resolution1400X1050,
        [SelectionOption("1600x900")] Resolution1600X900,
        [SelectionOption("1600x1050")] Resolution1600X1050,
        [SelectionOption("1776x1000")] Resolution1776X1000,
        [SelectionOption("1920x1080")] Resolution1920X1080,
        [SelectionOption("2560x1440")] Resolution2560X1440,
        [SelectionOption("3840x2160")] Resolution3840X2160,
    }
}
