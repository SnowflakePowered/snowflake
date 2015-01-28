using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Utility;
using Xunit;
namespace Snowflake.Utility.Tests
{
    public class BiDictionaryTests
    {
        [Fact]
        public void BiDictionaryCreation_Test(){
            var dictionary = new BiDictionary<object, object>();
            Assert.NotNull(dictionary);
        }
        [Fact]
        public void BiDictionaryAdd_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("test", "test");
            Assert.NotEmpty(dictionary);
        }
        [Fact]
        public void BiDictionaryRemove_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("test", "test");
            dictionary.Remove("test");
            Assert.Empty(dictionary);
        }
        [Fact]
        public void BiDictionaryClear_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("test", "test");
            dictionary.Add("test2", "test2");
            dictionary.Clear();
            Assert.Empty(dictionary);
        }
        [Fact]
        public void BiDictionaryReverse_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("Key", "Value");
            Assert.Equal("Key", dictionary.Reverse["Value"]);
        }
        [Fact]
        public void BiDictionaryReverseAdd_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Reverse.Add("test", "test");
            Assert.NotEmpty(dictionary);
        }
        [Fact]
        public void BiDictionaryReverseRemove_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Reverse.Add("test", "test");
            dictionary.Reverse.Remove("test");
            Assert.Empty(dictionary);
        }
        [Fact]
        public void BiDictionaryReverseClear_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Reverse.Add("test", "test");
            dictionary.Reverse.Add("test2", "test2");
            dictionary.Reverse.Clear();
            Assert.Empty(dictionary);
        }
        [Fact]
        public void BiDictionaryContains_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("Key", "Value");
            Assert.True(dictionary.ContainsKey("Key"));
        }
        [Fact]
        public void BiDictionaryValues_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("Key", "Value");
            Assert.Equal("Value", dictionary.Values.First());
        }
        [Fact]
        public void BiDictionaryKeys_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Add("Key", "Value");
            Assert.Equal("Key", dictionary.Keys.First());
        }
        [Fact]
        public void BiDictionaryIndexer_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary["test"] = "test";
            Assert.NotEmpty(dictionary);
        }

        [Fact]
        public void BiDictionarySet_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary["First"] = "Second";
            Assert.Equal("Second", dictionary["First"]);
        }
        [Fact]
        public void BiDictionaryReverseSet_Test()
        {
            var dictionary = new BiDictionary<object, object>();
            dictionary.Reverse["Second"] = "First";
            Assert.Equal("Second", dictionary["First"]);
        }

    }
}
