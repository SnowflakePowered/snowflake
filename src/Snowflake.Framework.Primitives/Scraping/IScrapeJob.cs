using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowflake.Scraping
{
    public interface IScrapeJob
    {
        ISeedRootContext Context { get; }
        IEnumerable<ICuller> Cullers { get; }
        Guid JobGuid { get; }
        IEnumerable<IScraper> Scrapers { get; }
        void Cull();
        Task<bool> Proceed();
        Task<bool> Proceed(IEnumerable<SeedContent> seedsToAdd);
    }
}