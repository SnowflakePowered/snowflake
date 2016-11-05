﻿using Snowflake.Configuration;
using Snowflake.Configuration.Attributes;
using Snowflake.DynamicConfiguration;


//autogenerated using generate_retroarch.py
namespace Snowflake.Configuration.Tests
{
    public interface IVideoConfiguration : IConfigurationSection<IVideoConfiguration>
    {

        [ConfigurationOption("video_black_frame_insertion", false, DisplayName = "Video Black Frame Insertion")]
        bool VideoBlackFrameInsertion { get; set; }

        //unknown
        [ConfigurationOption("video_context_driver", "", DisplayName = "Video Context Driver", Private = true)]
        string VideoContextDriver { get; set; }

        [ConfigurationOption("video_crop_overscan", true, DisplayName = "Crop Overscan")]
        bool VideoCropOverscan { get; set; }

        [ConfigurationOption("video_disable_composition", false, DisplayName = "Video Disable Composition", Private = true)]
        bool VideoDisableComposition { get; set; }

        [ConfigurationOption("video_driver", VideoDriver.Vulkan, DisplayName = "Video Driver")]
        VideoDriver VideoDriver { get; set; }

        [ConfigurationOption("video_filter", "", DisplayName = "Video Filter", Private = true, IsPath = true)]
        string VideoFilter { get; set; }

        [ConfigurationOption("video_filter_dir", "default", DisplayName = "Video Filter Dir", IsPath = true, Private = true)]
        string VideoFilterDir { get; set; }

        [ConfigurationOption("video_font_enable", true, DisplayName = "Video Font Enable", Private = true)]
        bool VideoFontEnable { get; set; }

        [ConfigurationOption("video_font_path", "", DisplayName = "Video Font Path", Private = true)]
        string VideoFontPath { get; set; }

        [ConfigurationOption("video_font_size", 32.000000, DisplayName = "Video Font Size", Private = true)]
        double VideoFontSize { get; set; }

        [ConfigurationOption("video_force_srgb_disable", false, DisplayName = "Force-disable sRGB FBO")]
        bool VideoForceSrgbDisable { get; set; }

        [ConfigurationOption("video_frame_delay", 0, DisplayName = "Frame Delay", Max = 15)]
        int VideoFrameDelay { get; set; }

        [ConfigurationOption("video_fullscreen", false, DisplayName = "Enable Fullscreen", Simple = true)]
        bool VideoFullscreen { get; set; }

        [ConfigurationOption("video_fullscreen_x", 0, DisplayName = "Video Fullscreen X", Private = true)]
        int VideoFullscreenX { get; set; }

        [ConfigurationOption("video_fullscreen_y", 0, DisplayName = "Video Fullscreen Y", Private = true)]
        int VideoFullscreenY { get; set; }

        [ConfigurationOption("video_gpu_screenshot", true, DisplayName = "Enable GPU Screenshot")]
        bool VideoGpuScreenshot { get; set; }

        [ConfigurationOption("video_hard_sync", false, DisplayName = "Hard GPU Sync")]
        bool VideoHardSync { get; set; }

        [ConfigurationOption("video_hard_sync_frames", 0, DisplayName = "Hard GPU Sync Frames", Max = 3)]
        int VideoHardSyncFrames { get; set; }

        [ConfigurationOption("video_message_color", "ffff00", DisplayName = "Video Message Color", Private = true)]
        string VideoMessageColor { get; set; }

        [ConfigurationOption("video_message_pos_x", 0.050000, DisplayName = "Video Message Pos X", Private = true)]
        double VideoMessagePosX { get; set; }

        [ConfigurationOption("video_message_pos_y", 0.050000, DisplayName = "Video Message Pos Y", Private = true)]
        double VideoMessagePosY { get; set; }

        [ConfigurationOption("video_monitor_index", 0, DisplayName = "Video Monitor Index", Private = true)]
        int VideoMonitorIndex { get; set; }

        [ConfigurationOption("video_refresh_rate", 59.950001, DisplayName = "Video Refresh Rate", Private = true)]
        double VideoRefreshRate { get; set; }

        [ConfigurationOption("video_rotation", VideoRotation.Normal, DisplayName = "Video Rotation")]
        VideoRotation VideoRotation { get; set; }

        [ConfigurationOption("video_scale", 3.000000, DisplayName = "Windowed Scale", Simple = true, Min = 1, Max = 10)]
        double VideoScale { get; set; }

        [ConfigurationOption("video_scale_integer", false, DisplayName = "Integer Scaling", Simple = true)]
        bool VideoScaleInteger { get; set; }

        [ConfigurationOption("video_shader_enable", true, DisplayName = "Enable Shaders", Simple = true)]
        bool VideoShaderEnable { get; set; }

        [ConfigurationOption("video_shader", "", DisplayName = "Video Shader Path", IsPath = true, Private = true)]
        string VideoShaderPath { get; set; }

        [ConfigurationOption("video_shared_context", false, DisplayName = "Video Shared Context", Private = true)]
        bool VideoSharedContext { get; set; }

        [ConfigurationOption("video_smooth", true, DisplayName = "Smooth Video", Simple = true)]
        bool VideoSmooth { get; set; }

        [ConfigurationOption("video_swap_interval", 1, DisplayName = "VSync Swap Interval", Max = 4, Min = 1)]
        int VideoSwapInterval { get; set; }

        [ConfigurationOption("video_threaded", false, DisplayName = "Enable Threaded Video")]
        bool VideoThreaded { get; set; }

        [ConfigurationOption("video_vsync", false, DisplayName = "VSync")]
        bool VideoVsync { get; set; }

        [ConfigurationOption("video_windowed_fullscreen", true, DisplayName = "Use Windowed Fullscreen")]
        bool VideoWindowedFullscreen { get; set; }

        [ConfigurationOption("gamma_correction", false, DisplayName = "Enable Gamma Correction")]
        bool GammaCorrection { get; set; }

        //not sure what this does.
        [ConfigurationOption("current_resolution_id", 0, DisplayName = "Current Resolution Id", Private = true)]
        int CurrentResolutionId { get; set; }

 
        //the custom viewport options will not be usable because aspect ratio index custom is not part of the enum.
        [ConfigurationOption("custom_viewport_height", 720, DisplayName = "Custom Viewport Height", Private = true)]
        int CustomViewportHeight { get; set; }

        [ConfigurationOption("custom_viewport_width", 960, DisplayName = "Custom Viewport Width", Private = true)]
        int CustomViewportWidth { get; set; }

        [ConfigurationOption("custom_viewport_x", 0, DisplayName = "Custom Viewport X", Private = true)]
        int CustomViewportX { get; set; }

        [ConfigurationOption("custom_viewport_y", 0, DisplayName = "Custom Viewport Y", Private = true)]
        int CustomViewportY { get; set; }

        // This will never be used because the config aspect ratio index is not accessible in the enum by design
        [ConfigurationOption("video_aspect_ratio", -1.000000, DisplayName = "Video Aspect Ratio (deprecated)", Private = true)]
        double VideoAspectRatio { get; set; }

        [ConfigurationOption("video_aspect_ratio_auto", false, DisplayName = "Video Aspect Ratio Auto (deprecated)", Private = true)]
        bool VideoAspectRatioAuto { get; set; }

        [ConfigurationOption("fps_show", false, DisplayName = "Show FPS Overlay", Simple = true)]
        bool FpsShow { get; set; }
    }
}

