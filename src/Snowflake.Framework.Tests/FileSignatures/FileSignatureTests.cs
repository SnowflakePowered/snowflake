using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Romfile;
using Snowflake.Stone.FileSignatures.Formats.CDI;
using Snowflake.Stone.FileSignatures.Nintendo;
using Snowflake.Stone.FileSignatures.Sega;
using Snowflake.Stone.FileSignatures.Sony;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Stone.FileSignatures.Tests
{
    public class FileSignatureTests
    {
        [Theory]
        [InlineData(typeof(NintendoEntertainmentSystemiNesFileSignature), "test.nes")]
        [InlineData(typeof(NintendoEntertainmentSystemUnifFileSignature), "scanline.unif")]
        [InlineData(typeof(SuperNintendoHeaderlessFileSignature), "bsnesdemo_v1.sfc")]
        [InlineData(typeof(SuperNintendoSmcHeaderFileSignature), "bsnesdemo_v1.smc")]
        [InlineData(typeof(GameboyAdvancedFileSignature), "suite.gba")]
        [InlineData(typeof(NintendoDSFileSignature), "slot1launch.nds", typeof(GameboyAdvancedFileSignature))]
        [InlineData(typeof(GameboyFileSignature), "flappyboy.gb", typeof(GameboyColorFileSignature))]
        [InlineData(typeof(GameboyColorFileSignature), "infinity.gbc", typeof(GameboyFileSignature))]
        [InlineData(typeof(Nintendo64ByteswappedFileSignature), "rx-mm64.v64", typeof(Nintendo64BigEndianFileSignature))]
        [InlineData(typeof(Nintendo64LittleEndianFileSignature), "rx-mm64.n64", typeof(Nintendo64ByteswappedFileSignature))]
        [InlineData(typeof(Nintendo64BigEndianFileSignature), "setscreenntsc.z64", typeof(Nintendo64LittleEndianFileSignature))]
        [InlineData(typeof(GamecubeIso9660FileSignature), "gctest.iso", typeof(WiiIso9660FileSignature))]
        [InlineData(typeof(PlaystationPortableIso9660FileSignature), "psptest.iso")]
        [InlineData(typeof(Playstation2CDRomFileSignature), "guitarfun.bin")]
        [InlineData(typeof(PlaystationCDRomFileSignature), "psxtest.bin", typeof(Playstation2CDRomFileSignature))]
        [InlineData(typeof(Playstation2Iso9660FileSignature), "ps2test.iso", typeof(Playstation2CDRomFileSignature))]
        [InlineData(typeof(SegaGenesisFileSignature), "SpriteMaskingTestRom.gen", typeof(Sega32XFileSignature))]
        [InlineData(typeof(Sega32XFileSignature), "devstertest12.32X", typeof(SegaGenesisFileSignature))]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest.gg", typeof(SegaMasterSystemFileSignature))]
        [InlineData(typeof(SegaGameGearFileSignature), "exporttest.gg", typeof(SegaMasterSystemFileSignature))]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest.sms", typeof(SegaGameGearFileSignature))]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest2.sms", typeof(SegaGameGearFileSignature))]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest2.gg", typeof(SegaMasterSystemFileSignature))]
        [InlineData(typeof(SegaDreamcastRawDiscFileSignature), "dctest.bin", typeof(SegaDreamcastDiscJugglerFileSignature))]
        [InlineData(typeof(SegaDreamcastDiscJugglerFileSignature), "240pSuite.cdi", typeof(SegaDreamcastRawDiscFileSignature))]

        public void Verify_Test(Type fileSignature, string filename, Type exclusionFs = null)
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.{filename}");
            IFileSignature signature = (IFileSignature)Activator.CreateInstance(fileSignature);
            Assert.True(signature.HeaderSignatureMatches(testStream));

            if (exclusionFs != null)
            {
                IFileSignature exclusion = (IFileSignature)Activator.CreateInstance(exclusionFs);
                Assert.False(exclusion.HeaderSignatureMatches(testStream));
            }
        }

        [Theory]
        [InlineData(typeof(NintendoEntertainmentSystemiNesFileSignature), "test.nes", null)]
        [InlineData(typeof(NintendoEntertainmentSystemUnifFileSignature), "scanline.unif", null)]
        [InlineData(typeof(SuperNintendoHeaderlessFileSignature), "bsnesdemo_v1.sfc", "SNES")]
        [InlineData(typeof(SuperNintendoSmcHeaderFileSignature), "bsnesdemo_v1.smc", "SNES")]
        [InlineData(typeof(GameboyAdvancedFileSignature), "suite.gba", "TEST")]
        [InlineData(typeof(NintendoDSFileSignature), "slot1launch.nds", "TWL1")]
        [InlineData(typeof(GameboyFileSignature), "flappyboy.gb", null)]
        [InlineData(typeof(GameboyColorFileSignature), "infinity.gbc", null)]
        [InlineData(typeof(Nintendo64ByteswappedFileSignature), "rx-mm64.v64", "NMME")]
        [InlineData(typeof(Nintendo64LittleEndianFileSignature), "rx-mm64.n64", "NMME")]
        [InlineData(typeof(Nintendo64BigEndianFileSignature), "setscreenntsc.z64", "")]
        [InlineData(typeof(GamecubeIso9660FileSignature), "gctest.iso", "ZZZZ00")]
        [InlineData(typeof(PlaystationPortableIso9660FileSignature), "psptest.iso", "NPUG80224")]
        [InlineData(typeof(Playstation2CDRomFileSignature), "guitarfun.bin", "")]
        [InlineData(typeof(PlaystationCDRomFileSignature), "psxtest.bin", "SLUS-01156")]
        [InlineData(typeof(Playstation2Iso9660FileSignature), "ps2test.iso", "SLUS-21008")]
        [InlineData(typeof(SegaGenesisFileSignature), "SpriteMaskingTestRom.gen", "T-XXXXX")]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest.gg", "3204")]
        [InlineData(typeof(SegaGameGearFileSignature), "exporttest.gg", "2408")]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest.sms", "7076")]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest2.sms", "27047")]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest2.gg", "3336")]
        [InlineData(typeof(Sega32XFileSignature), "devstertest12.32X", "MK-0000 -00")]
        [InlineData(typeof(SegaDreamcastRawDiscFileSignature), "dctest.bin", "MK-51035")]
        [InlineData(typeof(SegaDreamcastDiscJugglerFileSignature), "240pSuite.cdi", "T0001")]

        public void Verify_Serial(Type fileSignature, string filename, string expected)
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.{filename}");

            IFileSignature signature = (IFileSignature)Activator.CreateInstance(fileSignature);
            Assert.Equal(expected, signature.GetSerial(testStream));
        }


        [Theory]
        [InlineData(typeof(NintendoEntertainmentSystemiNesFileSignature), "test.nes", null)]
        [InlineData(typeof(NintendoEntertainmentSystemUnifFileSignature), "scanline.unif", null)]
        [InlineData(typeof(SuperNintendoHeaderlessFileSignature), "bsnesdemo_v1.sfc", "bsnes test demo")]
        [InlineData(typeof(SuperNintendoSmcHeaderFileSignature), "bsnesdemo_v1.smc", "bsnes test demo")]
        [InlineData(typeof(GameboyAdvancedFileSignature), "suite.gba", "TESTSUITE")]
        [InlineData(typeof(NintendoDSFileSignature), "slot1launch.nds", "TWLMENUPP-S1")]
        [InlineData(typeof(GameboyFileSignature), "flappyboy.gb", "FLAPPYBOY")]
        [InlineData(typeof(GameboyColorFileSignature), "infinity.gbc", "INFINITY")]
        [InlineData(typeof(Nintendo64ByteswappedFileSignature), "rx-mm64.v64", "Manic Miner 64")]
        [InlineData(typeof(Nintendo64LittleEndianFileSignature), "rx-mm64.n64", "Manic Miner 64")]
        [InlineData(typeof(Nintendo64BigEndianFileSignature), "setscreenntsc.z64", "N64 PROGRAM TITLE")]
        [InlineData(typeof(PlaystationPortableIso9660FileSignature), "psptest.iso", "Everyday Shooter")]
        [InlineData(typeof(GamecubeIso9660FileSignature), "gctest.iso", "GAMECUBE")]
        [InlineData(typeof(Playstation2CDRomFileSignature), "guitarfun.bin", "PS2TEST")]
        [InlineData(typeof(PlaystationCDRomFileSignature), "psxtest.bin", "VALKYRIE")]
        [InlineData(typeof(Playstation2Iso9660FileSignature), "ps2test.iso", "KATAMARI")]
        [InlineData(typeof(SegaGenesisFileSignature), "SpriteMaskingTestRom.gen", "Sprite Masking Test ROM")]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest.gg", null)]
        [InlineData(typeof(SegaGameGearFileSignature), "exporttest.gg", null)]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest.sms", null)]
        [InlineData(typeof(SegaGameGearFileSignature), "jptest2.gg", null)]
        [InlineData(typeof(SegaMasterSystemFileSignature), "exporttest2.sms", null)]
        [InlineData(typeof(Sega32XFileSignature), "devstertest12.32X", "32X GAME")]
        [InlineData(typeof(SegaDreamcastRawDiscFileSignature), "dctest.bin", "CRAZY TAXI")]
        [InlineData(typeof(SegaDreamcastDiscJugglerFileSignature), "240pSuite.cdi", "240P TEST SUITE")]

        public void Verify_InternalName(Type fileSignature, string filename, string expected)
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.{filename}");
            IFileSignature signature = (IFileSignature)Activator.CreateInstance(fileSignature);
            Assert.Equal(expected, signature.GetInternalName(testStream));
        }
    }
}
