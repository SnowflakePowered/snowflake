using System;
using Xunit;

namespace Snowflake.Scraper.Tests
{
    public class GameImagesResultTests
    {
        [Fact]
        public void GameImagesResultCreation_Test()
        {
            Assert.NotNull(new GameImagesResult());
        }
        [Theory]
        [InlineData(GameImageType.IMAGE_BOXART_FRONT)]
        [InlineData(GameImageType.IMAGE_BOXART_BACK)]
        [InlineData(GameImageType.IMAGE_BOXART_FULL)]
        [InlineData(GameImageType.IMAGE_FANART)]
        [InlineData(GameImageType.IMAGE_SCREENSHOT)]
        public void GameImagesAdd_Test(GameImageType type)
        {
            var gIR = new GameImagesResult();
            gIR.AddFromUrl(type, new Uri("http://localhost"));
            switch (type)
            {
                case GameImageType.IMAGE_BOXART_BACK:
                case GameImageType.IMAGE_BOXART_FRONT:
                case GameImageType.IMAGE_BOXART_FULL:
                    Assert.NotEmpty(gIR.Boxarts);
                    break;
                case GameImageType.IMAGE_FANART:
                    Assert.NotEmpty(gIR.Fanarts);
                    break;
                case GameImageType.IMAGE_SCREENSHOT:
                    Assert.NotEmpty(gIR.Screenshots);
                    break;
            }
        }
    }
}
