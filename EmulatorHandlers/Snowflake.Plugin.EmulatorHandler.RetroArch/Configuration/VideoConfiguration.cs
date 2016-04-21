using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.Plugin.EmulatorHandler.RetroArch.Selections.AudioConfiguration;
using Snowflake.Plugin.EmulatorHandler.RetroArch.Selections.VideoConfiguration;

//autogenerated using generate_retroarch.py
namespace Snowflake.Plugin.EmulatorHandler.RetroArch.Configuration
{
    public class VideoConfiguration : ConfigurationSection
    {


        [ConfigurationOption("video_black_frame_insertion", DisplayName = "Video Black Frame Insertion")]
        public bool VideoBlackFrameInsertion { get; set; } = false;

        //unknown
        [ConfigurationOption("video_context_driver", DisplayName = "Video Context Driver", Private = true)]
        public string VideoContextDriver { get; set; } = "";

        [ConfigurationOption("video_crop_overscan", DisplayName = "Crop Overscan")]
        public bool VideoCropOverscan { get; set; } = true;

        [ConfigurationOption("video_disable_composition", DisplayName = "Video Disable Composition", Private = true)]
        public bool VideoDisableComposition { get; set; } = false;

        [ConfigurationOption("video_driver", DisplayName = "Video Driver", Simple = true)]
        public VideoDriver VideoDriver { get; set; } = VideoDriver.OpenGL;

        [ConfigurationOption("video_filter", DisplayName = "Video Filter", Private = true, FilePath = true)]
        public string VideoFilter { get; set; } = "";

        [ConfigurationOption("video_filter_dir", DisplayName = "Video Filter Dir", FilePath = true, Private = true)]
        public string VideoFilterDir { get; set; } = "default";

        [ConfigurationOption("video_font_enable", DisplayName = "Video Font Enable", Private = true)]
        public bool VideoFontEnable { get; set; } = true;

        [ConfigurationOption("video_font_path", DisplayName = "Video Font Path", Private = true)]
        public string VideoFontPath { get; set; } = "";

        [ConfigurationOption("video_font_size", DisplayName = "Video Font Size", Private = true)]
        public double VideoFontSize { get; set; } = 32.000000;

        [ConfigurationOption("video_force_srgb_disable", DisplayName = "Force-disable sRGB FBO")]
        public bool VideoForceSrgbDisable { get; set; } = false;

        [ConfigurationOption("video_frame_delay", DisplayName = "Frame Delay", Max = 15)]
        public int VideoFrameDelay { get; set; } = 0;

        [ConfigurationOption("video_fullscreen", DisplayName = "Enable Fullscreen", Simple = true)]
        public bool VideoFullscreen { get; set; } = false;

        [ConfigurationOption("video_fullscreen_x", DisplayName = "Video Fullscreen X", Private = true)]
        public int VideoFullscreenX { get; set; } = 0;

        [ConfigurationOption("video_fullscreen_y", DisplayName = "Video Fullscreen Y", Private = true)]
        public int VideoFullscreenY { get; set; } = 0;

        [ConfigurationOption("video_gpu_screenshot", DisplayName = "Enable GPU Screenshot")]
        public bool VideoGpuScreenshot { get; set; } = true;

        [ConfigurationOption("video_hard_sync", DisplayName = "Hard GPU Sync")]
        public bool VideoHardSync { get; set; } = false;

        [ConfigurationOption("video_hard_sync_frames", DisplayName = "Hard GPU Sync Frames", Max = 3)]
        public int VideoHardSyncFrames { get; set; } = 0;

        [ConfigurationOption("video_message_color", DisplayName = "Video Message Color", Private = true)]
        public string VideoMessageColor { get; set; } = "ffff00";

        [ConfigurationOption("video_message_pos_x", DisplayName = "Video Message Pos X", Private = true)]
        public double VideoMessagePosX { get; set; } = 0.050000;

        [ConfigurationOption("video_message_pos_y", DisplayName = "Video Message Pos Y", Private = true)]
        public double VideoMessagePosY { get; set; } = 0.050000;

        [ConfigurationOption("video_monitor_index", DisplayName = "Video Monitor Index", Private = true)]
        public int VideoMonitorIndex { get; set; } = 0;

        [ConfigurationOption("video_refresh_rate", DisplayName = "Video Refresh Rate", Private = true)]
        public double VideoRefreshRate { get; set; } = 59.950001;

        [ConfigurationOption("video_rotation", DisplayName = "Video Rotation")]
        public VideoRotation VideoRotation { get; set; } = VideoRotation.Normal;

        [ConfigurationOption("video_scale", DisplayName = "Windowed Scale", Simple = true, Min = 1, Max = 10)]
        public double VideoScale { get; set; } = 3.000000;

        [ConfigurationOption("video_scale_integer", DisplayName = "Integer Scaling", Simple = true)]
        public bool VideoScaleInteger { get; set; } = false;

        [ConfigurationOption("video_shader_enable", DisplayName = "Video Shader Enable", Private = true)]
        public bool VideoShaderEnable { get; set; } = false;

        [ConfigurationOption("video_shared_context", DisplayName = "Video Shared Context", Private = true)]
        public bool VideoSharedContext { get; set; } = false;

        [ConfigurationOption("video_smooth", DisplayName = "Smooth Video", Simple = true)]
        public bool VideoSmooth { get; set; } = true;

        [ConfigurationOption("video_swap_interval", DisplayName = "VSync Swap Interval", Max = 4, Min = 1)]
        public int VideoSwapInterval { get; set; } = 1;

        [ConfigurationOption("video_threaded", DisplayName = "Enable Threaded Video")]
        public bool VideoThreaded { get; set; } = false;

        [ConfigurationOption("video_vsync", DisplayName = "VSync")]
        public bool VideoVsync { get; set; } = true;

        [ConfigurationOption("video_windowed_fullscreen", DisplayName = "Use Windowed Fullscreen")]
        public bool VideoWindowedFullscreen { get; set; } = true;

        [ConfigurationOption("gamma_correction", DisplayName = "Enable Gamma Correction")]
        public bool GammaCorrection { get; set; } = false;

        //not sure what this does.
        [ConfigurationOption("current_resolution_id", DisplayName = "Current Resolution Id", Private = true)]
        public int CurrentResolutionId { get; set; } = 0;

        [ConfigurationOption("aspect_ratio_index", DisplayName = "Aspect Ratio", Simple = true)]
        public AspectRatioIndex AspectRatioIndex { get; set; } = AspectRatioIndex.CoreProvided;

        //the custom viewport options will not be usable because aspect ratio index custom is not part of the enum.
        [ConfigurationOption("custom_viewport_height", DisplayName = "Custom Viewport Height", Private = true)]
        public int CustomViewportHeight { get; set; } = 720;

        [ConfigurationOption("custom_viewport_width", DisplayName = "Custom Viewport Width", Private = true)]
        public int CustomViewportWidth { get; set; } = 960;

        [ConfigurationOption("custom_viewport_x", DisplayName = "Custom Viewport X", Private = true)]
        public int CustomViewportX { get; set; } = 0;

        [ConfigurationOption("custom_viewport_y", DisplayName = "Custom Viewport Y", Private = true)]
        public int CustomViewportY { get; set; } = 0;

        // This will never be used because the config aspect ratio index is not accessible in the enum by design
        [ConfigurationOption("video_aspect_ratio", DisplayName = "Video Aspect Ratio (deprecated)", Private = true)]
        public double VideoAspectRatio { get; set; } = -1.000000;

        [ConfigurationOption("video_aspect_ratio_auto", DisplayName = "Video Aspect Ratio Auto (deprecated)", Private = true)]
        public bool VideoAspectRatioAuto { get; set; } = false;

        [ConfigurationOption("fps_show", DisplayName = "Show FPS Overlay", Simple = true)]
        public bool FpsShow { get; set; } = false;

        public VideoConfiguration() : base("video", "Video Options", "retroarch.cfg")
        {

        }

    }
}
