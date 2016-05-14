using System;
using System.Drawing;
using System.IO;
using Snowflake.Tests;
using Xunit;

namespace Snowflake.Caching.Tests
{
    public class KeyedImageCacheTests
    {
        [Fact]
        public void ImageAdd_Tests()
        {
            var cache = new KeyedImageCache(Path.GetTempPath());
            using (Stream stream = TestUtilities.GetResource("GameCache.600x600.gif"))
            {
                Image image = Image.FromStream(stream);
                var images = cache.Add(image, Guid.Empty, ImageTypes.Generic);
                foreach (var frecord in images)
                {
                    Assert.True(File.Exists(frecord.FilePath));
                    Assert.Equal(frecord.Record, Guid.Empty);
                }
            }
        }
    }
}
