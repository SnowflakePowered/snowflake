using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextCompleteMessage
    : EventMessage
    {
        public OnScrapeContextCompleteMessage(Guid jobId, ScrapeContextCompletePayload payload)
            : base(CreateEventDescription(jobId), payload)
        {
        }

        private static EventDescription CreateEventDescription(Guid jobId)
        {
            return new EventDescription("onScrapeContextComplete",
                new ArgumentNode("jobId", new StringValueNode(jobId.ToString("N"))));
        }
    }
}
