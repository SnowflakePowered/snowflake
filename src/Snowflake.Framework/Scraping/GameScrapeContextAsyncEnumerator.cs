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
            this.CullersRun = false;
        }

        public IEnumerable<ISeed> Current { get; private set; }

        private GameScrapeContext Context { get; }
        public CancellationToken Token { get; }
        private bool CullersRun { get; set; }

        public ValueTask DisposeAsync() => default;

        public async ValueTask<bool> MoveNextAsync()
        {
            // is this the correct value for this?
            if (this.Token.IsCancellationRequested) return false; 
            
            bool retVal = await this.Context.Proceed();
            this.Current = this.Context.Context.GetUnculled();

            if (!retVal && !this.CullersRun)
            {
                this.Context.Cull();
                this.CullersRun = true;
                return true;
            }
            return retVal;
        }
    }
}
