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
            Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", hash);
        }
        [Fact]
        public void Hash_SHA1_Test()
        {
            var fileStream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
            string hash = FileHash.GetSHA1(fileStream);
            Assert.Equal("DA39A3EE5E6B4B0D3255BFEF95601890AFD80709", hash);
        }
    }
}
