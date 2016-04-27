using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Configuration;
using Snowflake.Configuration.Input;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;

namespace Snowflake.Plugin.EmulatorHandler.RetroArch
{
    public class HotkeyConfiguration 
    {

        [HotkeyOption("input_toggle_fast_forward", InputOptionType.KeyboardKey, DisplayName = "Input Toggle Fast Forward")]
        public IMappedControllerElement InputToggleFastForward { get; set; }

        [HotkeyOption("input_toggle_fast_forward_btn", InputOptionType.ControllerElement, DisplayName = "Input Toggle Fast Forward Btn")]
        public IMappedControllerElement InputToggleFastForwardBtn { get; set; }

        [HotkeyOption("input_toggle_fast_forward_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Toggle Fast Forward Axis")]
        public IMappedControllerElement InputToggleFastForwardAxis { get; set; }

        [HotkeyOption("input_hold_fast_forward", InputOptionType.KeyboardKey, DisplayName = "Input Hold Fast Forward")]
        public IMappedControllerElement InputHoldFastForward { get; set; }

        [HotkeyOption("input_hold_fast_forward_btn", InputOptionType.ControllerElement, DisplayName = "Input Hold Fast Forward Btn")]
        public IMappedControllerElement InputHoldFastForwardBtn { get; set; }

        [HotkeyOption("input_hold_fast_forward_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Hold Fast Forward Axis")]
        public IMappedControllerElement InputHoldFastForwardAxis { get; set; }

        [HotkeyOption("input_load_state", InputOptionType.KeyboardKey, DisplayName = "Input Load State")]
        public IMappedControllerElement InputLoadState { get; set; }

        [HotkeyOption("input_load_state_btn", InputOptionType.ControllerElement, DisplayName = "Input Load State Btn")]
        public IMappedControllerElement InputLoadStateBtn { get; set; }

        [HotkeyOption("input_load_state_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Load State Axis")]
        public IMappedControllerElement InputLoadStateAxis { get; set; }

        [HotkeyOption("input_save_state", InputOptionType.KeyboardKey, DisplayName = "Input Save State")]
        public IMappedControllerElement InputSaveState { get; set; }

        [HotkeyOption("input_save_state_btn", InputOptionType.ControllerElement, DisplayName = "Input Save State Btn")]
        public IMappedControllerElement InputSaveStateBtn { get; set; }

        [HotkeyOption("input_save_state_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Save State Axis")]
        public IMappedControllerElement InputSaveStateAxis { get; set; }

        [HotkeyOption("input_toggle_fullscreen", InputOptionType.KeyboardKey, DisplayName = "Input Toggle Fullscreen")]
        public IMappedControllerElement InputToggleFullscreen { get; set; }

        [HotkeyOption("input_toggle_fullscreen_btn", InputOptionType.ControllerElement, DisplayName = "Input Toggle Fullscreen Btn")]
        public IMappedControllerElement InputToggleFullscreenBtn { get; set; }

        [HotkeyOption("input_toggle_fullscreen_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Toggle Fullscreen Axis")]
        public IMappedControllerElement InputToggleFullscreenAxis { get; set; }

        [HotkeyOption("input_exit_emulator", InputOptionType.KeyboardKey, DisplayName = "Input Exit Emulator")]
        public IMappedControllerElement InputExitEmulator { get; set; }

        [HotkeyOption("input_exit_emulator_btn", InputOptionType.ControllerElement, DisplayName = "Input Exit Emulator Btn")]
        public IMappedControllerElement InputExitEmulatorBtn { get; set; }

        [HotkeyOption("input_exit_emulator_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Exit Emulator Axis")]
        public IMappedControllerElement InputExitEmulatorAxis { get; set; }

        [HotkeyOption("input_state_slot_increase", InputOptionType.KeyboardKey, DisplayName = "Input State Slot Increase")]
        public IMappedControllerElement InputStateSlotIncrease { get; set; }

        [HotkeyOption("input_state_slot_increase_btn", InputOptionType.ControllerElement, DisplayName = "Input State Slot Increase Btn")]
        public IMappedControllerElement InputStateSlotIncreaseBtn { get; set; }

        [HotkeyOption("input_state_slot_increase_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input State Slot Increase Axis")]
        public IMappedControllerElement InputStateSlotIncreaseAxis { get; set; }

        [HotkeyOption("input_state_slot_decrease", InputOptionType.KeyboardKey, DisplayName = "Input State Slot Decrease")]
        public IMappedControllerElement InputStateSlotDecrease { get; set; }

        [HotkeyOption("input_state_slot_decrease_btn", InputOptionType.ControllerElement, DisplayName = "Input State Slot Decrease Btn")]
        public IMappedControllerElement InputStateSlotDecreaseBtn { get; set; }

        [HotkeyOption("input_state_slot_decrease_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input State Slot Decrease Axis")]
        public IMappedControllerElement InputStateSlotDecreaseAxis { get; set; }

        [HotkeyOption("input_rewind", InputOptionType.KeyboardKey, DisplayName = "Input Rewind")]
        public IMappedControllerElement InputRewind { get; set; }

        [HotkeyOption("input_rewind_btn", InputOptionType.ControllerElement, DisplayName = "Input Rewind Btn")]
        public IMappedControllerElement InputRewindBtn { get; set; }

        [HotkeyOption("input_rewind_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Rewind Axis")]
        public IMappedControllerElement InputRewindAxis { get; set; }

        [HotkeyOption("input_movie_record_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Movie Record Toggle")]
        public IMappedControllerElement InputMovieRecordToggle { get; set; }

        [HotkeyOption("input_movie_record_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Movie Record Toggle Btn")]
        public IMappedControllerElement InputMovieRecordToggleBtn { get; set; }

        [HotkeyOption("input_movie_record_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Movie Record Toggle Axis")]
        public IMappedControllerElement InputMovieRecordToggleAxis { get; set; }

        [HotkeyOption("input_pause_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Pause Toggle")]
        public IMappedControllerElement InputPauseToggle { get; set; }

        [HotkeyOption("input_pause_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Pause Toggle Btn")]
        public IMappedControllerElement InputPauseToggleBtn { get; set; }

        [HotkeyOption("input_pause_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Pause Toggle Axis")]
        public IMappedControllerElement InputPauseToggleAxis { get; set; }

        [HotkeyOption("input_frame_advance", InputOptionType.KeyboardKey, DisplayName = "Input Frame Advance")]
        public IMappedControllerElement InputFrameAdvance { get; set; }

        [HotkeyOption("input_frame_advance_btn", InputOptionType.ControllerElement, DisplayName = "Input Frame Advance Btn")]
        public IMappedControllerElement InputFrameAdvanceBtn { get; set; }

        [HotkeyOption("input_frame_advance_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Frame Advance Axis")]
        public IMappedControllerElement InputFrameAdvanceAxis { get; set; }

        [HotkeyOption("input_reset", InputOptionType.KeyboardKey, DisplayName = "Input Reset")]
        public IMappedControllerElement InputReset { get; set; }

        [HotkeyOption("input_reset_btn", InputOptionType.ControllerElement, DisplayName = "Input Reset Btn")]
        public IMappedControllerElement InputResetBtn { get; set; }

        [HotkeyOption("input_reset_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Reset Axis")]
        public IMappedControllerElement InputResetAxis { get; set; }

        [HotkeyOption("input_shader_next", InputOptionType.KeyboardKey, DisplayName = "Input Shader Next")]
        public IMappedControllerElement InputShaderNext { get; set; }

        [HotkeyOption("input_shader_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Shader Next Btn")]
        public IMappedControllerElement InputShaderNextBtn { get; set; }

        [HotkeyOption("input_shader_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Shader Next Axis")]
        public IMappedControllerElement InputShaderNextAxis { get; set; }

        [HotkeyOption("input_shader_prev", InputOptionType.KeyboardKey, DisplayName = "Input Shader Prev")]
        public IMappedControllerElement InputShaderPrev { get; set; }

        [HotkeyOption("input_shader_prev_btn", InputOptionType.ControllerElement, DisplayName = "Input Shader Prev Btn")]
        public IMappedControllerElement InputShaderPrevBtn { get; set; }

        [HotkeyOption("input_shader_prev_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Shader Prev Axis")]
        public IMappedControllerElement InputShaderPrevAxis { get; set; }

        [HotkeyOption("input_cheat_index_plus", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Index Plus")]
        public IMappedControllerElement InputCheatIndexPlus { get; set; }

        [HotkeyOption("input_cheat_index_plus_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Index Plus Btn")]
        public IMappedControllerElement InputCheatIndexPlusBtn { get; set; }

        [HotkeyOption("input_cheat_index_plus_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Index Plus Axis")]
        public IMappedControllerElement InputCheatIndexPlusAxis { get; set; }

        [HotkeyOption("input_cheat_index_minus", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Index Minus")]
        public IMappedControllerElement InputCheatIndexMinus { get; set; }

        [HotkeyOption("input_cheat_index_minus_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Index Minus Btn")]
        public IMappedControllerElement InputCheatIndexMinusBtn { get; set; }

        [HotkeyOption("input_cheat_index_minus_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Index Minus Axis")]
        public IMappedControllerElement InputCheatIndexMinusAxis { get; set; }

        [HotkeyOption("input_cheat_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Cheat Toggle")]
        public IMappedControllerElement InputCheatToggle { get; set; }

        [HotkeyOption("input_cheat_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Toggle Btn")]
        public IMappedControllerElement InputCheatToggleBtn { get; set; }

        [HotkeyOption("input_cheat_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Toggle Axis")]
        public IMappedControllerElement InputCheatToggleAxis { get; set; }

        [HotkeyOption("input_screenshot", InputOptionType.KeyboardKey, DisplayName = "Input Screenshot")]
        public IMappedControllerElement InputScreenshot { get; set; }

        [HotkeyOption("input_screenshot_btn", InputOptionType.ControllerElement, DisplayName = "Input Screenshot Btn")]
        public IMappedControllerElement InputScreenshotBtn { get; set; }

        [HotkeyOption("input_screenshot_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Screenshot Axis")]
        public IMappedControllerElement InputScreenshotAxis { get; set; }

        [HotkeyOption("input_audio_mute", InputOptionType.KeyboardKey, DisplayName = "Input Audio Mute")]
        public IMappedControllerElement InputAudioMute { get; set; }

        [HotkeyOption("input_audio_mute_btn", InputOptionType.ControllerElement, DisplayName = "Input Audio Mute Btn")]
        public IMappedControllerElement InputAudioMuteBtn { get; set; }

        [HotkeyOption("input_audio_mute_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Audio Mute Axis")]
        public IMappedControllerElement InputAudioMuteAxis { get; set; }

        [HotkeyOption("input_osk_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Osk Toggle")]
        public IMappedControllerElement InputOskToggle { get; set; }

        [HotkeyOption("input_osk_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Osk Toggle Btn")]
        public IMappedControllerElement InputOskToggleBtn { get; set; }

        [HotkeyOption("input_osk_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Osk Toggle Axis")]
        public IMappedControllerElement InputOskToggleAxis { get; set; }

        [HotkeyOption("input_netplay_flip_players", InputOptionType.KeyboardKey, DisplayName = "Input Netplay Flip Players")]
        public IMappedControllerElement InputNetplayFlipPlayers { get; set; }

        [HotkeyOption("input_netplay_flip_players_btn", InputOptionType.ControllerElement, DisplayName = "Input Netplay Flip Players Btn")]
        public IMappedControllerElement InputNetplayFlipPlayersBtn { get; set; }

        [HotkeyOption("input_netplay_flip_players_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Netplay Flip Players Axis")]
        public IMappedControllerElement InputNetplayFlipPlayersAxis { get; set; }

        [HotkeyOption("input_slowmotion", InputOptionType.KeyboardKey, DisplayName = "Input Slowmotion")]
        public IMappedControllerElement InputSlowmotion { get; set; }

        [HotkeyOption("input_slowmotion_btn", InputOptionType.ControllerElement, DisplayName = "Input Slowmotion Btn")]
        public IMappedControllerElement InputSlowmotionBtn { get; set; }

        [HotkeyOption("input_slowmotion_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Slowmotion Axis")]
        public IMappedControllerElement InputSlowmotionAxis { get; set; }

        [HotkeyOption("input_enable_hotkey", InputOptionType.KeyboardKey, DisplayName = "Input Enable Hotkey")]
        public IMappedControllerElement InputEnableHotkey { get; set; }

        [HotkeyOption("input_enable_hotkey_btn", InputOptionType.ControllerElement, DisplayName = "Input Enable Hotkey Btn")]
        public IMappedControllerElement InputEnableHotkeyBtn { get; set; }

        [HotkeyOption("input_enable_hotkey_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Enable Hotkey Axis")]
        public IMappedControllerElement InputEnableHotkeyAxis { get; set; }

        [HotkeyOption("input_volume_up", InputOptionType.KeyboardKey, DisplayName = "Input Volume Up")]
        public IMappedControllerElement InputVolumeUp { get; set; }

        [HotkeyOption("input_volume_up_btn", InputOptionType.ControllerElement, DisplayName = "Input Volume Up Btn")]
        public IMappedControllerElement InputVolumeUpBtn { get; set; }

        [HotkeyOption("input_volume_up_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Volume Up Axis")]
        public IMappedControllerElement InputVolumeUpAxis { get; set; }

        [HotkeyOption("input_volume_down", InputOptionType.KeyboardKey, DisplayName = "Input Volume Down")]
        public IMappedControllerElement InputVolumeDown { get; set; }

        [HotkeyOption("input_volume_down_btn", InputOptionType.ControllerElement, DisplayName = "Input Volume Down Btn")]
        public IMappedControllerElement InputVolumeDownBtn { get; set; }

        [HotkeyOption("input_volume_down_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Volume Down Axis")]
        public IMappedControllerElement InputVolumeDownAxis { get; set; }

        [HotkeyOption("input_overlay_next", InputOptionType.KeyboardKey, DisplayName = "Input Overlay Next")]
        public IMappedControllerElement InputOverlayNext { get; set; }

        [HotkeyOption("input_overlay_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Overlay Next Btn")]
        public IMappedControllerElement InputOverlayNextBtn { get; set; }

        [HotkeyOption("input_overlay_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Overlay Next Axis")]
        public IMappedControllerElement InputOverlayNextAxis { get; set; }

        [HotkeyOption("input_disk_eject_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Disk Eject Toggle")]
        public IMappedControllerElement InputDiskEjectToggle { get; set; }

        [HotkeyOption("input_disk_eject_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Eject Toggle Btn")]
        public IMappedControllerElement InputDiskEjectToggleBtn { get; set; }

        [HotkeyOption("input_disk_eject_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Eject Toggle Axis")]
        public IMappedControllerElement InputDiskEjectToggleAxis { get; set; }

        [HotkeyOption("input_disk_next", InputOptionType.KeyboardKey, DisplayName = "Input Disk Next")]
        public IMappedControllerElement InputDiskNext { get; set; }

        [HotkeyOption("input_disk_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Next Btn")]
        public IMappedControllerElement InputDiskNextBtn { get; set; }

        [HotkeyOption("input_disk_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Next Axis")]
        public IMappedControllerElement InputDiskNextAxis { get; set; }

        [HotkeyOption("input_disk_prev", InputOptionType.KeyboardKey, DisplayName = "Input Disk Prev")]
        public IMappedControllerElement InputDiskPrev { get; set; }

        [HotkeyOption("input_disk_prev_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Prev Btn")]
        public IMappedControllerElement InputDiskPrevBtn { get; set; }

        [HotkeyOption("input_disk_prev_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Prev Axis")]
        public IMappedControllerElement InputDiskPrevAxis { get; set; }

        [HotkeyOption("input_grab_mouse_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Grab Mouse Toggle")]
        public IMappedControllerElement InputGrabMouseToggle { get; set; }

        [HotkeyOption("input_grab_mouse_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Grab Mouse Toggle Btn")]
        public IMappedControllerElement InputGrabMouseToggleBtn { get; set; }

        [HotkeyOption("input_grab_mouse_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Grab Mouse Toggle Axis")]
        public IMappedControllerElement InputGrabMouseToggleAxis { get; set; }

        [HotkeyOption("input_menu_toggle", InputOptionType.KeyboardKey, DisplayName = "Input Menu Toggle")]
        public IMappedControllerElement InputMenuToggle { get; set; }

        [HotkeyOption("input_menu_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Menu Toggle Btn")]
        public IMappedControllerElement InputMenuToggleBtn { get; set; }

        [HotkeyOption("input_menu_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Menu Toggle Axis")]
        public IMappedControllerElement InputMenuToggleAxis { get; set; }



    }
}
