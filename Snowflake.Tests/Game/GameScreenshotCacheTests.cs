using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Game;
using System.Drawing;
using System.IO;
using Xunit;
using Snowflake.Tests;
namespace Snowflake.Game.Tests
{
    public class GameScreenshotCacheTests
    {
        [Fact]
        public void GameScreenshotCacheCreation_Test()
        {
            string testKey = Guid.NewGuid().ToString();
            IGameScreenshotCache screenCache = new GameScreenshotCache(testKey);
            Assert.True(Directory.Exists(Path.Combine(screenCache.RootPath, screenCache.CacheKey)));
        }
        [Fact]
        public void AddScreenshotUri_Test()
        {
            string testKey = Guid.NewGuid().ToString();
            IGameScreenshotCache screenCache = new GameScreenshotCache(testKey);
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                image.Save(tempFileName);
                image.Dispose();
                screenCache.AddScreenshot(new Uri(tempFileName));
            }
            Assert.NotEmpty(screenCache.ScreenshotCollection);
        }
        [Fact]
        public void AddScreenshotImage_Test()
        {
            string testKey = Guid.NewGuid().ToString();
            IGameScreenshotCache screenCache = new GameScreenshotCache(testKey);
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                screenCache.AddScreenshot(image);
            }
            Assert.NotEmpty(screenCache.ScreenshotCollection);
        }
        [Fact]
        public void RemoveScreenshot_Test()
        {
            string testKey = Guid.NewGuid().ToString();
            IGameScreenshotCache screenCache = new GameScreenshotCache(testKey);
            string tempFileName = Path.GetTempFileName();
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                screenCache.AddScreenshot(image);
            }
            screenCache.RemoveScreenshot(0);
            Assert.Empty(screenCache.ScreenshotCollection);
        }


        
    }
}
