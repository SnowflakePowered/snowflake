using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Snowflake.Game;
using System.Drawing;
using System.Drawing.Imaging;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Game.Tests
{
    public class GameMediaCacheTests
    {
        [Fact]
        public void GameMediaCacheCreation_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            Assert.True(Directory.Exists(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey)));
        }
        
        [Fact]
        public void SetBoxartFront_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                image.Save(tempFileName);
                image.Dispose();
                mediaCache.SetBoxartFront(new Uri(tempFileName));
            }
            Assert.NotNull(mediaCache.GetBoxartFrontImage());
        }
        [Fact]
        public void SetBoxartBack_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                image.Save(tempFileName);
                image.Dispose();
                mediaCache.SetBoxartBack(new Uri(tempFileName));
            }
            Assert.NotNull(mediaCache.GetBoxartBackImage());
        }
        [Fact]
        public void SetBoxartFull_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                image.Save(tempFileName);
                image.Dispose();
                mediaCache.SetBoxartFull(new Uri(tempFileName));
            }
            Assert.NotNull(mediaCache.GetBoxartFullImage());
        }
        [Fact]
        public void SetGameFanart_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                image.Save(tempFileName);
                image.Dispose();
                mediaCache.SetGameFanart(new Uri(tempFileName));
            }
            Assert.NotNull(mediaCache.GetGameFanartImage());
        }

        [Fact]
        public void ResizeImage_Test()
        {
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                Image resizedImage = GameMediaCache.ResizeImage(image, 0.5);
                Assert.Equal(new Size(300, 300), resizedImage.Size);
                image.Dispose();
                resizedImage.Dispose();
            }
        }

        [Fact]
        public void SetGameMusic_Test()
        {
            IGameMediaCache mediaCache = new GameMediaCache("TESTKey");
            string tempFileName = Path.GetTempFileName() + ".mp3";
            using (Stream stream = TestUtilities.GetResource("GameCache.dmmy.mp3"))
            {
                using (FileStream writeStream = new FileStream(tempFileName, FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(writeStream);
                }
            }
            Uri musicUri = new Uri(tempFileName);
            mediaCache.SetGameMusic(musicUri);
            Assert.NotNull(mediaCache.GameMusicFileName);
        }
    }
}
