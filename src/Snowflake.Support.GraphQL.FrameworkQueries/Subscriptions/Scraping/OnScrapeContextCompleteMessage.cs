using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextCompleteMessage
    : SingleIdentifierChannelMessage
    {
        public OnScrapeContextCompleteMessage(Guid jobId, ScrapeContextCompletePayload payload)
            : base(jobId, payload, "onScrapeContextComplete", nameof(jobId))
        {
        }
    }
}
