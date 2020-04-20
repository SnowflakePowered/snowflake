using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextStepMessage
    : EventMessage
    {
        public OnScrapeContextStepMessage(Guid jobId, ScrapeContextStepPayload payload)
            : base(CreateEventDescription(jobId), payload)
        {
        }

        private static EventDescription CreateEventDescription(Guid jobId)
        {
            return new EventDescription("onScrapeContextStep",
                new ArgumentNode("jobId", new StringValueNode(jobId.ToString("N"))));
        }
    }
}
