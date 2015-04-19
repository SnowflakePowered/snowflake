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
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey);
            Assert.True(Directory.Exists(Path.Combine(mediaCache.RootPath, mediaCache.CacheKey)));
        }
        
        [Fact]
        public void SetBoxartFront_Test()
        {
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
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
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
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
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
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
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
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

        [Theory]
        [InlineData("dmmy.mp3")]
        [InlineData("dmmy.ogg")]
        public void SetGameMusic_Test(string fileName)
        {
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
            string tempFileName = Path.GetTempFileName() + Path.GetExtension(fileName);
            using (Stream stream = TestUtilities.GetResource("GameCache." + fileName))
            {
                using (FileStream writeStream = new FileStream(tempFileName, FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(writeStream);
                }
            }
            Uri musicUri = new Uri(tempFileName);
            mediaCache.SetGameMusic(musicUri);
            Assert.Equal(Path.GetExtension(fileName), Path.GetExtension(mediaCache.GameMusicFileName));
        }

      
        [Theory]
        [InlineData("test.mp4")]
        [InlineData("test.webm")]
        public void SetGameVideo_Test(string fileName)
        {
            string testKey = Guid.NewGuid().ToString();
            IGameMediaCache mediaCache = new GameMediaCache(testKey); 
            string tempFileName = Path.GetTempFileName() + Path.GetExtension(fileName);
            using (Stream stream = TestUtilities.GetResource("GameCache." + fileName))
            {
                using (FileStream writeStream = new FileStream(tempFileName, FileMode.Create))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(writeStream);
                }
            }
            Uri musicUri = new Uri(tempFileName);
            mediaCache.SetGameVideo(musicUri);
            Assert.Equal(Path.GetExtension(fileName), Path.GetExtension(mediaCache.GameVideoFileName));
        }
    }
}
