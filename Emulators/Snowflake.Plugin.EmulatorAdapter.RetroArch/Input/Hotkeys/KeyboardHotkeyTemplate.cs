using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Hotkey;
using Snowflake.Configuration.Input;
using Snowflake.Configuration.Input.Hotkey;
using Snowflake.Input.Controller;


namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys
{
    public class KeyboardHotkeyTemplate : HotkeyTemplate
    {

        [HotkeyOption("input_toggle_fast_forward", InputOptionType.KeyboardKey,
            DisplayName = "Input Toggle Fast Forward", Private = true)]
        public KeyboardKey InputToggleFastForward { get; set; } = KeyboardKey.KeyComma;

        [HotkeyOption("input_hold_fast_forward", InputOptionType.KeyboardKey, DisplayName = "Input Hold Fast Forward", Private = true)]
        public KeyboardKey InputHoldFastForward { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_load_state", InputOptionType.KeyboardKey, DisplayName = "Input Load State", Private = true)]
        public KeyboardKey InputLoadState { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_save_state", InputOptionType.KeyboardKey, DisplayName = "Input Save State", Private = true)]
        public KeyboardKey InputSaveState { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_toggle_fullscreen", InputOptionType.KeyboardKey, DisplayName = "Input Toggle Fullscreen", Private = true)]
        public KeyboardKey InputToggleFullscreen { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_exit_emulator", InputOptionType.KeyboardKey, DisplayName = "Input Exit Emulator", Private = true)]
        public KeyboardKey InputExitEmulator { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_state_slot_increase", InputOptionType.KeyboardKey, DisplayName = "Input State Slot Increase", Private = true)]
        public KeyboardKey InputStateSlotIncrease { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_state_slot_decrease", InputOptionType.KeyboardKey, DisplayName = "Input State Slot Decrease", Private = true)]
        public KeyboardKey InputStateSlotDecrease { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_rewind", InputOptionType.KeyboardKey, DisplayName = "Input Rewind", Private = true)]
        public KeyboardKey InputRewind { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_movie_record_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Movie Record Toggle", Private = true)]
        public KeyboardKey InputMovieRecordToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_pause_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Pause Toggle", Private = true)]
        public KeyboardKey InputPauseToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_frame_advance", InputOptionType.KeyboardKey, DisplayName = "Input Frame Advance", Private = true)]
        public KeyboardKey InputFrameAdvance { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_reset", InputOptionType.KeyboardKey, DisplayName = "Input Reset", Private = true)]
        public KeyboardKey InputReset { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_shader_next", InputOptionType.KeyboardKey, DisplayName = "Input Shader Next", Private = true)]
        public KeyboardKey InputShaderNext { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_shader_prev", InputOptionType.KeyboardKey, DisplayName = "Input Shader Prev", Private = true)]
        public KeyboardKey InputShaderPrev { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_cheat_index_plus", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Index Plus", Private = true)]
        public KeyboardKey InputCheatIndexPlus { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_cheat_index_minus", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Index Minus", Private = true)]
        public KeyboardKey InputCheatIndexMinus { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_cheat_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Toggle", Private = true)]
        public KeyboardKey InputCheatToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_screenshot", InputOptionType.KeyboardKey, DisplayName = "Input Screenshot", Private = true)]
        public KeyboardKey InputScreenshot { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_audio_mute", InputOptionType.KeyboardKey, DisplayName = "Input Audio Mute", Private = true)]
        public KeyboardKey InputAudioMute { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_osk_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Osk Toggle", Private = true)]
        public KeyboardKey InputOskToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_netplay_flip_players", InputOptionType.KeyboardKey, DisplayName = "Input Netplay Flip Players", Private = true)]
        public KeyboardKey InputNetplayFlipPlayers { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_slowmotion", InputOptionType.KeyboardKey, DisplayName = "Input Slowmotion", Private = true)]
        public KeyboardKey InputSlowmotion { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_enable_hotkey", InputOptionType.KeyboardKey, DisplayName = "Input Enable Hotkey", Private = true)]
        public KeyboardKey InputEnableHotkey { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_volume_up", InputOptionType.KeyboardKey, DisplayName = "Input Volume Up", Private = true)]
        public KeyboardKey InputVolumeUp { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_volume_down", InputOptionType.KeyboardKey, DisplayName = "Input Volume Down", Private = true)]
        public KeyboardKey InputVolumeDown { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_overlay_next", InputOptionType.KeyboardKey, DisplayName = "Input Overlay Next", Private = true)]
        public KeyboardKey InputOverlayNext { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_disk_eject_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Disk Eject Toggle", Private = true)]
        public KeyboardKey InputDiskEjectToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_disk_next", InputOptionType.KeyboardKey, DisplayName = "Input Disk Next", Private = true)]
        public KeyboardKey InputDiskNext { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_disk_prev", InputOptionType.KeyboardKey, DisplayName = "Input Disk Prev", Private = true)]
        public KeyboardKey InputDiskPrev { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_grab_mouse_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Grab Mouse Toggle", Private = true)]
        public KeyboardKey InputGrabMouseToggle { get; set; } = KeyboardKey.KeyNone;

        [HotkeyOption("input_menu_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Menu Toggle", Private = true)]
        public KeyboardKey InputMenuToggle { get; set; } = KeyboardKey.KeyNone;

        public KeyboardHotkeyTemplate() : base("input", "retroarch.cfg", HotkeyTemplateType.KeyboardHotkeys)
        {
        }

    }

}

