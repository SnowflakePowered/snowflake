using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Snowflake.Stone.FileSignatures.Formats.CDI;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Stone.FileSignatures.Formats.Tests
{
    public class DiscJugglerFormatTests
    {
       //[Fact]
       // public void OpenBlockEquivalence_Test()
       // {
       //     using var testStream = TestUtilities.GetResource($"TestRoms.240pSuite.cdi");
       //     var disc = new DiscJugglerDisc(testStream);
       //     var ip1 = Encoding.UTF8.GetString(disc(disc.Sessions[1], 1)).Trim('\0').Trim();
       //     using var byteReader = new BinaryReader(disc.OpenBlock(0));
       //     var ip2 = Encoding.UTF8.GetString(byteReader.ReadBytes((int)byteReader.BaseStream.Length)).Trim('\0').Trim();
       //     Assert.Equal(ip1, ip2);
       // }

       // public void DiscJugglerFormatTest()
       // {
       //     using var testStream = TestUtilities.GetResource($"TestRoms.240pSuite.cdi");
       //     var disk = new DiscJugglerDisc(testStream);
       //     var dc = new CdiDreamcastDisc(disk);
       //     var ip = dc.GetMeta();
       //     var pvd = disk.GetISOPVD();
       //     var memoryStream = new MemoryStream();
       //     disk.OpenBlock(0).CopyTo(memoryStream);
       //     var bytes = memoryStream.ToArray();
       // }
    }
}
