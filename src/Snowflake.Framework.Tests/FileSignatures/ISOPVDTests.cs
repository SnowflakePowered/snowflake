using Snowflake.Stone.FileSignatures.Formats.CDXA;
using Snowflake.Tests;
using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace Snowflake.Stone.FileSignatures.Tests
{
    public class ISOPVDTests
    {
        [Fact]
        public void ISOPVDHeader_Test()
        {
            using var testStream = TestUtilities.GetResource($"TestRoms.psxtest.bin");
            var disc = new CDXADisc(testStream);
            var pvd = disc.GetISOPVD();
            Assert.Equal(1, pvd.TypeCode);
            Assert.Equal("CD001", pvd.StandardIdentifier);
            Assert.Equal(1, pvd.FileStructureVersion);
            Assert.Equal(18u, pvd.LPathTableLocation);
            Assert.Equal(10u, pvd.PathTableSize);
            Assert.Equal(22u, pvd.RootDirectoryLBA);
            Assert.Equal(34, pvd.RootDirectoryEntryBytes.Length);
            Assert.Equal("PLAYSTATION", pvd.SystemIdentifier.Trim());
            Assert.Equal("VALKYRIE", pvd.VolumeIdentifier.Trim());
            Assert.Equal(1, pvd.VolumeSetSize);
            Assert.Equal(2048, pvd.LogicalBlockSize);
        }
    }
}
