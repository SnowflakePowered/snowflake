using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public class GameScrapeContextAutoCullingAsyncEnumerator : IAsyncEnumerator<IEnumerable<ISeed>>
    {
        public GameScrapeContextAutoCullingAsyncEnumerator(GameScrapeContext context)
        {
            this.Current = Enumerable.Empty<ISeed>();
            this.Context = context;
        }

        public IEnumerable<ISeed> Current { get; private set; }

        private GameScrapeContext Context { get; }

        public ValueTask DisposeAsync()
        {
            // what do i do here? there's nothing to dispose!
            return new ValueTask();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            bool retVal = await this.Context.Proceed();
            this.Context.Cull();
            this.Current = this.Context.Context.GetUnculled();
            return retVal;
        }
    }
}
