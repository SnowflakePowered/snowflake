using Snowflake.Extensibility.Queueing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Snowflake.Remoting.Tests
{
    public class AsyncJobQueueTests
    {
        [Fact]
        public async Task AsyncJobQueue_Test()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>();
            var token = await tq.QueueJob(EmitStrings());
            string _none = tq.GetCurrent(token);
            Assert.Null(_none);
            (string val, bool next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);
            Assert.Equal(val, tq.GetCurrent(token));
            Assert.True(next);
            (val, next) = await tq.GetNext(token);
            Assert.True(next);
            Assert.Equal("Goodbye World", val);
            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
            Assert.Null(tq.GetCurrent(token));
        }

        [Fact]
        public async Task AsyncJobQueue_TestWithOwnToken()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>();
            Guid token = Guid.NewGuid();
            Assert.Equal(token, await tq.QueueJob(EmitStrings(), token));

            (string val, bool next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);

            Assert.True(next);
            (val, next) = await tq.GetNext(token);
            Assert.Equal("Goodbye World", val);
            Assert.True(next);
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

            int count = 0;

            await foreach (string nextval in tq.AsEnumerable(token))
            {
                Assert.Equal("Goodbye World", nextval);
                count++;
            }

            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);
            Assert.Equal(1, count);

            var token2 = await tq.QueueJob(EmitStrings());

            count = 0;
            await foreach (string nextval in tq.AsEnumerable(token2))
            {
                count++;
            }

            Assert.Equal(2, count);

        }

        [Fact]
        public async Task AsyncJobQueue_ExistentialTest()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>(false);
            var token = await tq.QueueJob(EmitStrings());
            Assert.True(tq.HasJob(token));
            Assert.Contains(token, tq.GetActiveJobs());
            Assert.Contains(token, tq.GetQueuedJobs());
            Assert.DoesNotContain(token, tq.GetZombieJobs());
            await foreach (string _ in tq.AsEnumerable(token)) { };
            Assert.DoesNotContain(token, tq.GetActiveJobs());
            Assert.Contains(token, tq.GetQueuedJobs());
            Assert.Contains(token, tq.GetZombieJobs());
        }

        [Fact]
        public async Task AsyncJobQueue_ContextTest()
        {
            IAsyncJobQueue<string> tq = new AsyncJobQueue<string>(false);
            var token = await tq.QueueJob(EmitStrings());
            (string val, bool next) = await tq.GetNext(token);
            Assert.Equal("Hello World", val);

            Assert.False(tq.TryRemoveSource(token, out var _));

            await foreach (string nextval in tq.AsEnumerable(token))
            {
                Assert.Equal("Goodbye World", nextval);
            }

            (val, next) = await tq.GetNext(token);
            Assert.False(next);
            Assert.Null(val);

            var original = tq.GetSource(token);
            await foreach (string nextval in original)
            {
                Assert.True(nextval == "Hello World" || nextval == "Goodbye World");
            }

            Assert.True(tq.TryRemoveSource(token, out var originalNext));

            await foreach (string nextval in originalNext)
            {
                Assert.True(nextval == "Hello World" || nextval == "Goodbye World");
            }

            Assert.False(tq.TryRemoveSource(token, out var _));
        }

        public static async IAsyncEnumerable<string> EmitStrings()
        {
            yield return "Hello World";
            yield return "Goodbye World";
        }
    }
}
