using System;
using System.Collections.Generic;
using System.IO;
using Snowflake.Persistence;
using Xunit;

namespace Snowflake.Utility.Tests
{
    public class SqliteKeyValueStoreTests
    {
        private class TestObject
        {
            public int SomeInt { get; set; }
            public string SomeString { get; set; }
            public bool SomeBoolean { get; set; }
            public Guid SomeGuid => Guid.Empty;
        }

        [Fact]
        public void CreateDatabase_Test()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            Assert.NotNull(kvStore);
        }

        [Theory]
        [InlineData("StringData")]
        [InlineData(true)]
        [InlineData(101)]
        public void InsertObject_Test(object testData)
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testData);
        }


        [Fact]
        public void InsertObject_IgnoreTest()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", "oldValue");
            kvStore.InsertObject("testKey", "newValue", true);
            Assert.Equal("oldValue", kvStore.GetObject<string>("testKey"));
        }

        [Fact]
        public void InsertObject_ReplaceTest()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", "oldValue");
            kvStore.InsertObject("testKey", "newValue");
            Assert.Equal("newValue", kvStore.GetObject<string>("testKey"));
        }

        [Theory]
        [InlineData(101, true, "StringData")]
        public void InsertObject_ComplexObjectTest(int someInt, bool someBool, string someString)
        {
            var testObject = new TestObject()
            {
                SomeInt = someInt,
                SomeBoolean = someBool,
                SomeString = someString,
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testObject);
        }

        [Fact]
        public void InsertObjects_Test()
        {
            IDictionary<string, string> testDict = new Dictionary<string, string>()
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"}
            };

            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObjects(testDict);
        }


        [Fact]
        public void InsertObjects_IgnoreTest()
        {
            IDictionary<string, string> testDict = new Dictionary<string, string>()
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"}
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObjects(testDict);

            testDict["Test1"] = "newTest";
            kvStore.InsertObjects(testDict, true);
            Assert.Equal("TEST1", kvStore.GetObject<string>("Test1"));
        }

        [Fact]
        public void InsertObjects_ReplaceTest()
        {
            IDictionary<string, string> testDict = new Dictionary<string, string>()
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"}
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObjects(testDict);

            testDict["Test1"] = "newTest";
            kvStore.InsertObjects(testDict);
            Assert.Equal("newTest", kvStore.GetObject<string>("Test1"));
        }

        [Fact]
        public void InsertObject_DictionaryTest()
        {
            IDictionary<string, string> testDict = new Dictionary<string, string>()
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"}
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testDict);
        }

        [Theory]
        [InlineData("StringData")]
        [InlineData(true)]
        [InlineData(101L)]
        public void GetObject_Test(object testData)
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testData);
            Assert.Equal(testData, kvStore.GetObject<object>("testKey"));
        }

        [Fact]
        public void GetObjects_Test()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey1", "test1");
            kvStore.InsertObject("testKey2", "test2");

            var retrievedObjects = kvStore.GetObjects<string>(new[] { "testKey1", "testKey2" });
            Assert.NotEmpty(retrievedObjects);
        }

        [Theory]
        [InlineData(101, true, "StringData")]
        public void GetObject_ComplexObjectTest(int someInt, bool someBool, string someString)
        {
            var testObject = new TestObject()
            {
                SomeInt = someInt,
                SomeBoolean = someBool,
                SomeString = someString
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testObject);
            var retrievedObject = kvStore.GetObject<TestObject>("testKey");
            Assert.Equal(testObject.SomeInt, retrievedObject.SomeInt);
            Assert.Equal(testObject.SomeBoolean, retrievedObject.SomeBoolean);
            Assert.Equal(testObject.SomeString, retrievedObject.SomeString);
            Assert.Equal(testObject.SomeGuid, Guid.Empty);
        }

        [Fact]
        public void GetObject_DictionaryTest()
        {
            IDictionary<string, string> testDict = new Dictionary<string, string>()
            {
                {"Test1", "TEST1"},
                {"Test2", "TEST2"}
            };
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey", testDict);
            var retrievedObject = kvStore.GetObject<IDictionary<string, string>>("testKey");
            Assert.NotEmpty(testDict);
            Assert.Equal(testDict["Test1"], retrievedObject["Test1"]);
            Assert.Equal(testDict["Test2"], retrievedObject["Test2"]);
        }

        [Fact]
        public void GetAllObjects()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey1", "test1");
            kvStore.InsertObject("testKey2", 1);
            var retrievedObjects = kvStore.GetAllObjects<string>();
            Assert.Equal("test1", retrievedObjects["testKey1"]);
            Assert.False(retrievedObjects.ContainsKey("testKey2"));
        }

        [Fact]
        public void DeleteObject_Test()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey1", "test1");
            kvStore.DeleteObject("testKey1");
            Assert.Null(kvStore.GetObject<string>("testKey1"));
        }

        [Fact]
        public void DeleteObjects_Test()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey1", "test1");
            kvStore.InsertObject("testKey2", "test2");

            kvStore.DeleteObjects(new[] {"testKey1", "testKey2"});
            var retrievedObjects = kvStore.GetObjects<string>(new[] { "testKey1", "testKey2" });
            Assert.Empty(retrievedObjects);
        }

        [Fact]
        public void DeleteAllObjects_Test()
        {
            var kvStore = new SqliteKeyValueStore(Path.GetTempFileName());
            kvStore.InsertObject("testKey1", "test1");
            kvStore.InsertObject("testKey2", "test2");
            kvStore.InsertObject("testKey3", 1);

            kvStore.DeleteAllObjects<string>();
            var retrievedObjects = kvStore.GetObjects<string>(new[] { "testKey1", "testKey2" });
            Assert.Empty(retrievedObjects);
            Assert.Equal(1, kvStore.GetObject<int>("testKey3"));
        }
    }
}
