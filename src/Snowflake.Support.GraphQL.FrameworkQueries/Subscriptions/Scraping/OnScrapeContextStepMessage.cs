using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextStepMessage
    : SingleIdentifierChannelMessage
    {
        public OnScrapeContextStepMessage(Guid jobId, ScrapeContextStepPayload payload)
            : base(jobId, payload, "onScrapeContextStep", nameof(jobId))
        {
        }
    }
}
