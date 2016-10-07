using System.IO;
using Xunit;

namespace Snowflake.Utility.Tests
{
    public class HashTests
    {
        [Fact]
        public void Hash_CRC32_Test()
        {
            var fileStream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
            string hash = FileHash.GetCRC32(fileStream);
            Assert.Equal("00000000", hash);
        }
        [Fact]
        public void Hash_MD5_Test()
        {
            var fileStream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
            string hash = FileHash.GetMD5(fileStream);
            Assert.Equal("d41d8cd98f00b204e9800998ecf8427e".ToUpper(), hash);
        }
        [Fact]
        public void Hash_SHA1_Test()
        {
            var fileStream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
            string hash = FileHash.GetSHA1(fileStream);
            Assert.Equal("da39a3ee5e6b4b0d3255bfef95601890afd80709".ToUpper(), hash);
        }
    }
}
