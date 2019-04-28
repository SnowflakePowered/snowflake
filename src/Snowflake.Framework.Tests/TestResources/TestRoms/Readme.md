# Test ROMs

The ROMs contained here for testing purposes were ascertained to be freely distributable but not necessarily open source. Regardless, efforts have been made to ensure that they are non-commercial or otherwise distributable without liability. If you wish for a file to be removed from the repository, please file an issue and it will be taken care of as soon as possible.

* `test.nes`
  * [nes-test-roms/MMC1_A12](https://github.com/christopherpow/nes-test-roms/tree/fc217a73fe77a0e0726e4e121155882f3fbc7b3b/MMC1_A12)
* `scanline.unif`
  * [nes-test-roms/scanline](https://github.com/christopherpow/nes-test-roms/blob/fc217a73fe77a0e0726e4e121155882f3fbc7b3b/scanline/scanline.unif)
* `bsnesdemo_v1.sfc`
  * [SNES Central](https://snescentral.com/article.php?id=1114)
  * `bsnesdemo_v1.smc` was created by adding 0x200 filler bytes to the beginning of `bsnesdemo-v1.sfc`.
* `suite.gba`
  * [mgba-emu/suite](https://github.com/mgba-emu/suite)
  * Modified with `gbafix` to put in a correct title and game code.
  * MIT Licensed
* `slot1launch.nds`
  * [DS-Homebrew/TWiLightMenu](https://github.com/DS-Homebrew/TWiLightMenu)
  * MIT Licensed
* `flappyboy.gb`
  * [bitnenfer/FlappyBoy](https://github.com/bitnenfer/FlappyBoy)
  * MIT Licensed
* `infinity.gbc`
  * [infinity-gbc](https://github.com/infinity-gbc/infinity)
  * CC BY-NC-ND Licensed 
* `setscreenntsc.z64`
  * [PeterLemon/N64](https://github.com/PeterLemon/N64)
* `rxmm64.v64`
  * [NESWorld](http://www.nesworld.com/article.php?system=n64&data=n64homebrew)
* `guitarfun.bin`
  * [PSXPlace](https://www.psx-place.com/resources/guitar-fun.558/)
  * Generated using cdgenPS2
* `SpriteMaskingTestRom.gen`
  * [SegaRetro](https://segaretro.org/Sprite_Masking_and_Overflow_Test_ROM)
* `devstertest12.32X`
  * [devster](http://devster.monkeeh.com/segapage.html)
* `240psuite.cdi`
  * [JunkerHq](http://junkerhq.net/xrgb/index.php?title=240p_test_suite)

# ROM Headers
The following files were created from backups of commercially available ROM files with all non header data removed from the ROM. Since little if any copyrighted game data still exists in these files, they are redistributed here for testing purposes. Obviously, none of these ROM files will run in any emulator or hardware, since all game data has been removed. This was only used as a final resort when freely available homebrew ROMs were not suitable for header confirmation purposes.

* `psptest.iso`, `psptest.cso`
  * Originally from a dump of **Everyday Shooter (USA)**.
  * All files except header files were deleted from the dump.
  * BOOT.BIN, EBOOT.BIN were zeroed out.
  * ICON0.PNG, PIC0.PNG, PIC1PNG were left untouched.
* `gctest.iso`
  * Originally from a dump of **Taxi 3 (France)**
  * Extracted the boot.bin, renamed to gctest.iso.
  * ID6 was modified to be ZZZZ00
  * Game title was modified to be GAMECUBE
* `psxtest.bin`
  * Originally from a dump of the first disk of **Valkyrie Profile (USA)**
  * `SLUS_011.56` was zeroed out
  * `SLUS_011.56` and `SYSTEM.CNF` were extracted, then rebuilt using PSx CD-Gen, then patched with Disc Patcher
* `ps2test.iso`
  * Originally from a dump of **Katamari Damacy (USA)**
  * `SLUS_210.08` was zeroed out
  * Files besides `SLUS_210.08` and `SYSTEM.CNF` were removed from the ISO
* `jptest.gg`
  * Originally from a dump of **G-LOC Air Battle** for the Game Gear
  * All data except `0x7FF0-0x7FFF` was filled with padding bytes `0xFF`.
  * Checksum is no longer valid.
* `exporttest.sms`
  * Originally from a dump of **Sonic The Hedgehog (USA)**
  * All data except `0x7FF0-0x7FFF` was filled with padding bytes `0xFF`.
  * Checksum is no longer valid.
* `exporttest2.sms`
  * Originally from a dump of **Predator 2 (Europe)**
  * All data except `0x7FF0-0x7FFF` was filled with padding bytes `0xFF`.
  * Checksum is no longer valid.
* `jptest2.gg`
  * Originally from a dump of **Aladdin** for the Game Gear
  * All data except `0x7FF0-0x7FFF` was filled with padding bytes `0xFF`.
  * Checksum is no longer valid.
* `exporttest.gg`
  * Originally from a dump of **Sonic The Hedgehog (USA)**
  * All data except `0x7FF0-0x7FFF` was filled with padding bytes `0xFF`.
  * Checksum is no longer valid.
* `dctest.bin`
  * Originally from a dump of **Crazy Taxi (USA)**
  * All data past `0x10F` was deleted.