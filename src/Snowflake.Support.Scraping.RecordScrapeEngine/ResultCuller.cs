using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snowflake.Extensibility;
using Snowflake.Scraping;
using Snowflake.Scraping.Extensibility;
using Snowflake.Support.Scraping.RecordScrapeEngine.Utility;
using Snowflake.Utility;

namespace Snowflake.Support.Scraping.RecordScrapeEngine
{
    [Plugin("RecordScrapeEngine-ResultCuller")]
    public class ResultCuller : Culler
    {
        public ResultCuller()
            : base(typeof(ResultCuller), "result")
        {
        }

        public override IEnumerable<ISeed> Filter(IEnumerable<ISeed> seedsToTrim, ISeedRootContext context)
        {
            var clientResult = seedsToTrim.FirstOrDefault(s => s.Source == ScrapeJob.ClientSeedSource);
            if (clientResult != null)
            {
                yield return clientResult;
                yield break;
            }

            var crc32Results = seedsToTrim.Where(s => context[s.Parent]?.Content.Type == "search_crc32");
            var mostDetailed = crc32Results.OrderByDescending(s => context.GetChildren(s).Count()).FirstOrDefault();

            if (mostDetailed != null)
            {
                yield return mostDetailed;
                yield break;
            }

            var titleResults = (from seed in seedsToTrim
                                let parent = context[seed.Parent]
                                where parent?.Content.Type == "search_title"
                                let title = context.GetChildren(seed).FirstOrDefault(s => s.Content.Type == "title")
                                where title != null
                                let r = title.Content.Value
                                let distance = r.CompareTitle(parent.Content.Value)
                                orderby distance
                                select seed).FirstOrDefault();

            if (titleResults != null)
            {
                yield return titleResults;
            }
        }
    }
}
