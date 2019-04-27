using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Romfile;
using Snowflake.Stone.FileSignatures.Nintendo;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.FileSignatures.Tests
{
    public class FileSignatureTests
    {
        [Theory]
        [InlineData(typeof(NintendoEntertainmentSystemiNesFileSignature), "test.nes")]
        [InlineData(typeof(NintendoEntertainmentSystemUnifFileSignature), "scanline.unif")]
        [InlineData(typeof(SuperNintendoHeaderlessFileSignature), "bsnesdemo_v1.sfc")]
        [InlineData(typeof(SuperNintendoSmcHeaderFileSignature), "bsnesdemo_v1.smc")]
        public void Verify_Test(Type fileSignature, string filename)
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.{filename}");
            IFileSignature signature = (IFileSignature)Activator.CreateInstance(fileSignature);
            Assert.True(signature.HeaderSignatureMatches(testStream));
        }

        [Theory]
        [InlineData(typeof(NintendoEntertainmentSystemiNesFileSignature), "test.nes", null)]
        [InlineData(typeof(NintendoEntertainmentSystemUnifFileSignature), "scanline.unif", null)]
        [InlineData(typeof(SuperNintendoHeaderlessFileSignature), "bsnesdemo_v1.sfc", "SNES")]
        [InlineData(typeof(SuperNintendoSmcHeaderFileSignature), "bsnesdemo_v1.smc", "SNES")]

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
        public void Verify_InternalName(Type fileSignature, string filename, string expected)
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.{filename}");
            IFileSignature signature = (IFileSignature)Activator.CreateInstance(fileSignature);
            Assert.Equal(expected, signature.GetInternalName(testStream));
        }
    }
}
