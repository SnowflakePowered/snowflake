using Snowflake.Extensibility.Queueing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Snowflake.Extensibility.Queueing.Tests
{
    public class AsyncJobQueueFactoryTests
    {
        [Fact]
        public void AsyncJobQueueFactoryIdempotency_Test()
        {
            var fac = new AsyncJobQueueFactory();
            var trueInt = fac.GetJobQueue<int>(true);
            Assert.Same(trueInt, fac.GetJobQueue<int>(true));
        }

        [Fact]
        public void AsyncJobQueueFactoryDifferentBehaviour_Test()
        {
            var fac = new AsyncJobQueueFactory();
            var trueInt = fac.GetJobQueue<int>(true);
            Assert.NotSame(trueInt, fac.GetJobQueue<int>(false));
        }

        [Fact]
        public void AsyncJobQueueFactoryDefaultAsyncEnumeratorType_Test()
        {
            var fac = new AsyncJobQueueFactory();
            Assert.Equal(typeof(AsyncJobQueue<int>), fac.GetJobQueue<int>(false).GetType());
        }

        [Fact]
        public void AsyncJobQueueFactoryCustomAsyncEnumeratorType_Test()
        {
            var fac = new AsyncJobQueueFactory();
            Assert.Equal(typeof(AsyncJobQueue<DummyAsyncEnumerable, int>), fac.GetJobQueue<DummyAsyncEnumerable, int>(false).GetType());
        }
    }

    public class DummyAsyncEnumerable : IAsyncEnumerable<int>
    {
        public IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
