using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    internal class GameScrapeContextAsyncEnumerator : IAsyncEnumerator<IEnumerable<ISeed>>
    {
        public GameScrapeContextAsyncEnumerator(GameScrapeContext context, CancellationToken token)
        {
            this.Current = Enumerable.Empty<ISeed>();
            this.Context = context;
            this.Token = token;
        }

        public IEnumerable<ISeed> Current { get; private set; }

        private GameScrapeContext Context { get; }
        public CancellationToken Token { get; }

        public ValueTask DisposeAsync()
        {
            // what do i do here? there's nothing to dispose!
            return new ValueTask();
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            // is this the correct value for this?
            if (this.Token.IsCancellationRequested) return false; 
            
            bool retVal = await this.Context.Proceed();
            this.Current = this.Context.Context.GetUnculled();
            return retVal;
        }
    }
}
