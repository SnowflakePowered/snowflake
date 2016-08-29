using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;


namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys
{
    public class RetroarchHotkeyTemplate : HotkeyTemplate
    {

        [HotkeyOption("input_enable_hotkey", ControllerConfigurationKey = "input_enable_hotkey_btn", AxisConfigurationKey = "input_enable_hotkey_axis", DisplayName = "Input Enable Hotkey", Private = true)]
        public override HotkeyTrigger ModifierTrigger { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_toggle_fast_forward", ControllerConfigurationKey = "input_toggle_fast_forward_btn", AxisConfigurationKey = "input_toggle_fast_forward_axis", DisplayName = "Input Toggle Fast Forward", Private = true)]
        public HotkeyTrigger InputToggleFastForward { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_hold_fast_forward", ControllerConfigurationKey = "input_hold_fast_forward_btn", AxisConfigurationKey = "input_hold_fast_forward_axis", DisplayName = "Input Hold Fast Forward", Private = true)]
        public HotkeyTrigger InputHoldFastForward { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_load_state", ControllerConfigurationKey = "input_load_state_btn", AxisConfigurationKey = "input_load_state_axis", DisplayName = "Input Load State", Private = true)]
        public HotkeyTrigger InputLoadState { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_save_state", ControllerConfigurationKey = "input_save_state_btn", AxisConfigurationKey = "input_save_state_axis", DisplayName = "Input Save State", Private = true)]
        public HotkeyTrigger InputSaveState { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_toggle_fullscreen", ControllerConfigurationKey = "input_toggle_fullscreen_btn", AxisConfigurationKey = "input_toggle_fullscreen_axis", DisplayName = "Input Toggle Fullscreen", Private = true)]
        public HotkeyTrigger InputToggleFullscreen { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_exit_emulator", ControllerConfigurationKey = "input_exit_emulator_btn", AxisConfigurationKey = "input_exit_emulator_axis", DisplayName = "Input Exit Emulator", Private = true)]
        public HotkeyTrigger InputExitEmulator { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_state_slot_increase", ControllerConfigurationKey = "input_state_slot_increase_btn", AxisConfigurationKey = "input_state_slot_increase_axis", DisplayName = "Input State Slot Increase", Private = true)]
        public HotkeyTrigger InputStateSlotIncrease { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_state_slot_decrease", ControllerConfigurationKey = "input_state_slot_decrease_btn", AxisConfigurationKey = "input_state_slot_decrease_axis", DisplayName = "Input State Slot Decrease", Private = true)]
        public HotkeyTrigger InputStateSlotDecrease { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_rewind", ControllerConfigurationKey = "input_rewind_btn", AxisConfigurationKey = "input_rewind_axis", DisplayName = "Input Rewind", Private = true)]
        public HotkeyTrigger InputRewind { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_movie_record_toggle", ControllerConfigurationKey = "input_movie_record_toggle_btn", AxisConfigurationKey = "input_movie_record_toggle_axis", DisplayName = "Input Movie Record Toggle", Private = true)]
        public HotkeyTrigger InputMovieRecordToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_pause_toggle", ControllerConfigurationKey = "input_pause_toggle_btn", AxisConfigurationKey = "input_pause_toggle_axis", DisplayName = "Input Pause Toggle", Private = true)]
        public HotkeyTrigger InputPauseToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_frame_advance", ControllerConfigurationKey = "input_frame_advance_btn", AxisConfigurationKey = "input_frame_advance_axis", DisplayName = "Input Frame Advance", Private = true)]
        public HotkeyTrigger InputFrameAdvance { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_reset", ControllerConfigurationKey = "input_reset_btn", AxisConfigurationKey = "input_reset_axis", DisplayName = "Input Reset", Private = true)]
        public HotkeyTrigger InputReset { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_shader_next", ControllerConfigurationKey = "input_shader_next_btn", AxisConfigurationKey = "input_shader_next_axis", DisplayName = "Input Shader Next", Private = true)]
        public HotkeyTrigger InputShaderNext { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_shader_prev", ControllerConfigurationKey = "input_shader_prev_btn", AxisConfigurationKey = "input_shader_prev_axis", DisplayName = "Input Shader Prev", Private = true)]
        public HotkeyTrigger InputShaderPrev { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_cheat_index_plus", ControllerConfigurationKey = "input_cheat_index_plus_btn", AxisConfigurationKey = "input_cheat_index_plus_axis", DisplayName = "Input Cheat Index Plus", Private = true)]
        public HotkeyTrigger InputCheatIndexPlus { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_cheat_index_minus", ControllerConfigurationKey = "input_cheat_index_minus_btn", AxisConfigurationKey = "input_cheat_index_minus_axis", DisplayName = "Input Cheat Index Minus", Private = true)]
        public HotkeyTrigger InputCheatIndexMinus { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_cheat_toggle", ControllerConfigurationKey = "input_cheat_toggle_btn", AxisConfigurationKey = "input_cheat_toggle_axis", DisplayName = "Input Cheat Toggle", Private = true)]
        public HotkeyTrigger InputCheatToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_screenshot", ControllerConfigurationKey = "input_screenshot_btn", AxisConfigurationKey = "input_screenshot_axis", DisplayName = "Input Screenshot", Private = true)]
        public HotkeyTrigger InputScreenshot { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_audio_mute", ControllerConfigurationKey = "input_audio_mute_btn", AxisConfigurationKey = "input_audio_mute_axis", DisplayName = "Input Audio Mute", Private = true)]
        public HotkeyTrigger InputAudioMute { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_osk_toggle", ControllerConfigurationKey = "input_osk_toggle_btn", AxisConfigurationKey = "input_osk_toggle_axis", DisplayName = "Input Osk Toggle", Private = true)]
        public HotkeyTrigger InputOskToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_netplay_flip_players", ControllerConfigurationKey = "input_netplay_flip_players_btn", AxisConfigurationKey = "input_netplay_flip_players_axis", DisplayName = "Input Netplay Flip Players", Private = true)]
        public HotkeyTrigger InputNetplayFlipPlayers { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_slowmotion", ControllerConfigurationKey = "input_slowmotion_btn", AxisConfigurationKey = "input_slowmotion_axis", DisplayName = "Input Slowmotion", Private = true)]
        public HotkeyTrigger InputSlowmotion { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_volume_up", ControllerConfigurationKey = "input_volume_up_btn", AxisConfigurationKey = "input_volume_up_axis", DisplayName = "Input Volume Up", Private = true)]
        public HotkeyTrigger InputVolumeUp { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_volume_down", ControllerConfigurationKey = "input_volume_down_btn", AxisConfigurationKey = "input_volume_down_axis", DisplayName = "Input Volume Down", Private = true)]
        public HotkeyTrigger InputVolumeDown { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_overlay_next", ControllerConfigurationKey = "input_overlay_next_btn", AxisConfigurationKey = "input_overlay_next_axis", DisplayName = "Input Overlay Next", Private = true)]
        public HotkeyTrigger InputOverlayNext { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_disk_eject_toggle", ControllerConfigurationKey = "input_disk_eject_toggle_btn", AxisConfigurationKey = "input_disk_eject_toggle_axis", DisplayName = "Input Disk Eject Toggle", Private = true)]
        public HotkeyTrigger InputDiskEjectToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_disk_next", ControllerConfigurationKey = "input_disk_next_btn", AxisConfigurationKey = "input_disk_next_axis", DisplayName = "Input Disk Next", Private = true)]
        public HotkeyTrigger InputDiskNext { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_disk_prev", ControllerConfigurationKey = "input_disk_prev_btn", AxisConfigurationKey = "input_disk_prev_axis", DisplayName = "Input Disk Prev", Private = true)]
        public HotkeyTrigger InputDiskPrev { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_grab_mouse_toggle", ControllerConfigurationKey = "input_grab_mouse_toggle_btn", AxisConfigurationKey = "input_grab_mouse_toggle_axis", DisplayName = "Input Grab Mouse Toggle", Private = true)]
        public HotkeyTrigger InputGrabMouseToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        [HotkeyOption("input_menu_toggle", ControllerConfigurationKey = "input_menu_toggle_btn", AxisConfigurationKey = "input_menu_toggle_axis", DisplayName = "Input Menu Toggle", Private = true)]
        public HotkeyTrigger InputMenuToggle { get; set; } = new HotkeyTrigger(KeyboardKey.KeyNone, ControllerElement.NoElement);

        public RetroarchHotkeyTemplate() : base("input")
        {
        }

    }

}

