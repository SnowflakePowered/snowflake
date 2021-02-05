﻿using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal class OnScrapeContextCompleteMessage
    : EventMessage<Guid>
    {
        public OnScrapeContextCompleteMessage(ScrapeContextCompletePayload payload)
            : base(payload.JobID, payload, "jobId")
        {
        }
    }
}
