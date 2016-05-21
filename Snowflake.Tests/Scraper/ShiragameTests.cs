using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Scraper.Shiragame.Tests
{
    public class ShiragameTests
    {
        [Theory]
        [InlineData("Super Mario Bros. (World).nes", "D445F698")]
        public void GetFromCRC32_Test(string title, string hash)
        {
            //D445F698
            IShiragameProvider shiragame = new ShiragameProvider("shiragame.db");
            var game = shiragame.GetFromCrc32(hash);
            Assert.Equal(title, game.FileName);
        }
        [Theory]
        [InlineData("Super Mario Bros. (World).nes", "8E3630186E35D477231BF8FD50E54CDD")]
        public void GetFromMD5_Test(string title, string hash)
        {
            IShiragameProvider shiragame = new ShiragameProvider("shiragame.db");
            var game = shiragame.GetFromMd5(hash);
            Assert.Equal(title, game.FileName);
        }
        [Theory]
        [InlineData("Super Mario Bros. (World).nes", "FACEE9C577A5262DBE33AC4930BB0B58C8C037F7")]
        public void GetFromSHA1_Test(string title, string hash)
        {
            IShiragameProvider shiragame = new ShiragameProvider("shiragame.db");
            var game = shiragame.GetFromSha1(hash);
            Assert.Equal(title, game.FileName);
        }

        [Theory]
        [InlineData("NINTENDO_3DS", "AKHP", "Kingdom Hearts 3D - Dream Drop Distance")]
        [InlineData("SEGA_SAT", "T-6802G", "2do Arukotoha Sand-R (Japan)")]
        public void GetSerial_Test(string platformId, string serial, string title)
        {
            //T-6802G
            IShiragameProvider shiragame = new ShiragameProvider("shiragame.db");
            var game = shiragame.GetFromSerial(platformId, serial);
            Assert.Equal(title, game.Title);
        }
    }
}
