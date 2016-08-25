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
using Snowflake.Input.Controller.Mapped;
namespace Snowflake.Plugin.EmulatorAdapter.RetroArch.Input.Hotkeys
{
    public class ControllerHotkeyTemplate : HotkeyTemplate
    {
        [HotkeyOption("input_toggle_fast_forward_btn", InputOptionType.ControllerElement, DisplayName = "Input Toggle Fast Forward Btn", Private = true)]
        public ControllerElement InputToggleFastForwardBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_toggle_fast_forward_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Toggle Fast Forward Axis", Private = true)]
        public ControllerElement InputToggleFastForwardAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_hold_fast_forward_btn", InputOptionType.ControllerElement, DisplayName = "Input Hold Fast Forward Btn", Private = true)]
        public ControllerElement InputHoldFastForwardBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_hold_fast_forward_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Hold Fast Forward Axis", Private = true)]
        public ControllerElement InputHoldFastForwardAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_load_state_btn", InputOptionType.ControllerElement, DisplayName = "Input Load State Btn", Private = true)]
        public ControllerElement InputLoadStateBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_load_state_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Load State Axis", Private = true)]
        public ControllerElement InputLoadStateAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_save_state_btn", InputOptionType.ControllerElement, DisplayName = "Input Save State Btn", Private = true)]
        public ControllerElement InputSaveStateBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_save_state_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Save State Axis", Private = true)]
        public ControllerElement InputSaveStateAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_toggle_fullscreen_btn", InputOptionType.ControllerElement, DisplayName = "Input Toggle Fullscreen Btn", Private = true)]
        public ControllerElement InputToggleFullscreenBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_toggle_fullscreen_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Toggle Fullscreen Axis", Private = true)]
        public ControllerElement InputToggleFullscreenAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_exit_emulator_btn", InputOptionType.ControllerElement, DisplayName = "Input Exit Emulator Btn", Private = true)]
        public ControllerElement InputExitEmulatorBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_exit_emulator_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Exit Emulator Axis", Private = true)]
        public ControllerElement InputExitEmulatorAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_state_slot_increase_btn", InputOptionType.ControllerElement, DisplayName = "Input State Slot Increase Btn", Private = true)]
        public ControllerElement InputStateSlotIncreaseBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_state_slot_increase_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input State Slot Increase Axis", Private = true)]
        public ControllerElement InputStateSlotIncreaseAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_state_slot_decrease_btn", InputOptionType.ControllerElement, DisplayName = "Input State Slot Decrease Btn", Private = true)]
        public ControllerElement InputStateSlotDecreaseBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_state_slot_decrease_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input State Slot Decrease Axis", Private = true)]
        public ControllerElement InputStateSlotDecreaseAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_rewind_btn", InputOptionType.ControllerElement, DisplayName = "Input Rewind Btn", Private = true)]
        public ControllerElement InputRewindBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_rewind_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Rewind Axis", Private = true)]
        public ControllerElement InputRewindAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_movie_record_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Movie Record Toggle Btn", Private = true)]
        public ControllerElement InputMovieRecordToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_movie_record_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Movie Record Toggle Axis", Private = true)]
        public ControllerElement InputMovieRecordToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_pause_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Pause Toggle Btn", Private = true)]
        public ControllerElement InputPauseToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_pause_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Pause Toggle Axis", Private = true)]
        public ControllerElement InputPauseToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_frame_advance_btn", InputOptionType.ControllerElement, DisplayName = "Input Frame Advance Btn", Private = true)]
        public ControllerElement InputFrameAdvanceBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_frame_advance_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Frame Advance Axis", Private = true)]
        public ControllerElement InputFrameAdvanceAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_reset_btn", InputOptionType.ControllerElement, DisplayName = "Input Reset Btn", Private = true)]
        public ControllerElement InputResetBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_reset_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Reset Axis", Private = true)]
        public ControllerElement InputResetAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_shader_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Shader Next Btn", Private = true)]
        public ControllerElement InputShaderNextBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_shader_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Shader Next Axis", Private = true)]
        public ControllerElement InputShaderNextAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_shader_prev_btn", InputOptionType.ControllerElement, DisplayName = "Input Shader Prev Btn", Private = true)]
        public ControllerElement InputShaderPrevBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_shader_prev_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Shader Prev Axis", Private = true)]
        public ControllerElement InputShaderPrevAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_index_plus_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Index Plus Btn", Private = true)]
        public ControllerElement InputCheatIndexPlusBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_index_plus_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Index Plus Axis", Private = true)]
        public ControllerElement InputCheatIndexPlusAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_index_minus_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Index Minus Btn", Private = true)]
        public ControllerElement InputCheatIndexMinusBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_index_minus_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Index Minus Axis", Private = true)]
        public ControllerElement InputCheatIndexMinusAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Cheat Toggle Btn", Private = true)]
        public ControllerElement InputCheatToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_cheat_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Cheat Toggle Axis", Private = true)]
        public ControllerElement InputCheatToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_screenshot_btn", InputOptionType.ControllerElement, DisplayName = "Input Screenshot Btn", Private = true)]
        public ControllerElement InputScreenshotBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_screenshot_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Screenshot Axis", Private = true)]
        public ControllerElement InputScreenshotAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_audio_mute_btn", InputOptionType.ControllerElement, DisplayName = "Input Audio Mute Btn", Private = true)]
        public ControllerElement InputAudioMuteBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_audio_mute_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Audio Mute Axis", Private = true)]
        public ControllerElement InputAudioMuteAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_osk_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Osk Toggle Btn", Private = true)]
        public ControllerElement InputOskToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_osk_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Osk Toggle Axis", Private = true)]
        public ControllerElement InputOskToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_netplay_flip_players_btn", InputOptionType.ControllerElement, DisplayName = "Input Netplay Flip Players Btn", Private = true)]
        public ControllerElement InputNetplayFlipPlayersBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_netplay_flip_players_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Netplay Flip Players Axis", Private = true)]
        public ControllerElement InputNetplayFlipPlayersAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_slowmotion_btn", InputOptionType.ControllerElement, DisplayName = "Input Slowmotion Btn", Private = true)]
        public ControllerElement InputSlowmotionBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_slowmotion_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Slowmotion Axis", Private = true)]
        public ControllerElement InputSlowmotionAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_enable_hotkey_btn", InputOptionType.ControllerElement, DisplayName = "Input Enable Hotkey Btn", Private = true)]
        public ControllerElement InputEnableHotkeyBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_enable_hotkey_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Enable Hotkey Axis", Private = true)]
        public ControllerElement InputEnableHotkeyAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_volume_up_btn", InputOptionType.ControllerElement, DisplayName = "Input Volume Up Btn", Private = true)]
        public ControllerElement InputVolumeUpBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_volume_up_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Volume Up Axis", Private = true)]
        public ControllerElement InputVolumeUpAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_volume_down_btn", InputOptionType.ControllerElement, DisplayName = "Input Volume Down Btn", Private = true)]
        public ControllerElement InputVolumeDownBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_volume_down_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Volume Down Axis", Private = true)]
        public ControllerElement InputVolumeDownAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_overlay_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Overlay Next Btn", Private = true)]
        public ControllerElement InputOverlayNextBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_overlay_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Overlay Next Axis", Private = true)]
        public ControllerElement InputOverlayNextAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_eject_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Eject Toggle Btn", Private = true)]
        public ControllerElement InputDiskEjectToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_eject_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Eject Toggle Axis", Private = true)]
        public ControllerElement InputDiskEjectToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_next_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Next Btn", Private = true)]
        public ControllerElement InputDiskNextBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_next_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Next Axis", Private = true)]
        public ControllerElement InputDiskNextAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_prev_btn", InputOptionType.ControllerElement, DisplayName = "Input Disk Prev Btn", Private = true)]
        public ControllerElement InputDiskPrevBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_disk_prev_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Disk Prev Axis", Private = true)]
        public ControllerElement InputDiskPrevAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_grab_mouse_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Grab Mouse Toggle Btn", Private = true)]
        public ControllerElement InputGrabMouseToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_grab_mouse_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Grab Mouse Toggle Axis", Private = true)]
        public ControllerElement InputGrabMouseToggleAxis { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_menu_toggle_btn", InputOptionType.ControllerElement, DisplayName = "Input Menu Toggle Btn", Private = true)]
        public ControllerElement InputMenuToggleBtn { get; set; } = ControllerElement.NoElement;

        [HotkeyOption("input_menu_toggle_axis", InputOptionType.ControllerElementAxes, DisplayName = "Input Menu Toggle Axis", Private = true)]
        public ControllerElement InputMenuToggleAxis { get; set; } = ControllerElement.NoElement;

        public ControllerHotkeyTemplate() : base("input", "retroarch.cfg", HotkeyTemplateType.ControllerHotkeys)
        {
        }

    }

}

