using Snowflake.Framework.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Framework.Remoting.Tests
{
    public class AsyncJobQueueTests
    {
        [Fact]
        public async Task AsyncJobQueue_Test()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>();
            var token = await tq.QueueJob(EmitStrings());
            (string val, bool next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);
            Assert.True(next);
            (_, next) = await tq.GetNext(token);
            Assert.False(next);
            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
        }

        [Fact]
        public async Task AsyncJobQueue_TestWithOwnToken()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>();
            Guid token = Guid.NewGuid();
            Assert.Equal(token, await tq.QueueJob(EmitStrings(), token));

            (string val, bool next) = await tq.GetNext(token);
            Assert.True(next);
            (_, next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);
            Assert.False(next);
            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
        }

        [Fact]
        public async Task AsyncJobQueue_EnumeratorTest()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>();
            var token = await tq.QueueJob(EmitStrings());
            (string val, bool next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);

            await foreach (string nextval in tq.GetEnumerable(token))
            {
                Assert.Equal("Goodbye World", nextval);
            }

            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
        }

        public static async IAsyncEnumerable<string> EmitStrings()
        {
            yield return "Hello World";
            yield return "Goodbye World";
        }
    }
}
