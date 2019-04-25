using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Snowflake.Scraping.Tests
{
    public class SeedContentEqualityTests
    {
        [Fact]
        public void SeedContentEqual_Test()
        {
            Assert.Equal(new SeedContent("Hello", "World"), new SeedContent("Hello", "World"));
            Assert.True(new SeedContent("Hello", "World") == new SeedContent("Hello", "World"));
            Assert.False(new SeedContent("Hello", "World") != new SeedContent("Hello", "World"));

            Assert.True(new SeedContent("Hello", "World").Equals(new SeedContent("Hello", "World")));
            Assert.Equal<SeedContent>(new SeedContent("Hello", "World"), ("Hello", "World"));
            Assert.True(new SeedContent("Hello", "World") == ("Hello", "World"));
            Assert.False(new SeedContent("Hello", "World") != ("Hello", "World"));

            Assert.NotEqual(new SeedContent("Hello", "World"), new SeedContent("Goodbye", "World"));
            Assert.False(new SeedContent("Hello", "World") == new SeedContent("Goodbye", "World"));
            Assert.True(new SeedContent("Hello", "World") != new SeedContent("Goodbye", "World"));
            Assert.False(new SeedContent("Hello", "World").Equals(new SeedContent("Goodbye", "World")));
            Assert.NotEqual<SeedContent>(new SeedContent("Hello", "World"), ("Goodbye", "World"));
            Assert.False(new SeedContent("Hello", "World") == ("Goodbye", "World"));
            Assert.True(new SeedContent("Hello", "World") != ("Goodbye", "World"));

            Assert.False(new SeedContent("Hello", "World") == null);
            Assert.True(new SeedContent("Hello", "World") != null);
            Assert.False(new SeedContent("Hello", "World").Equals(new object()));
            Assert.False(new SeedContent("Hello", "World").Equals(null));
        }
    }
}
