using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextStepMessage
    : EventMessage<Guid>
    {
        public OnScrapeContextStepMessage(ScrapeContextStepPayload payload)
            : base(payload.JobID, payload)
        {
        }
    }
}
