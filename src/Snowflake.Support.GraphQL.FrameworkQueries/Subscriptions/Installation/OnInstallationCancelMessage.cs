using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal class OnInstallationCancelMessage
    : EventMessage<Guid>
    {
        public OnInstallationCancelMessage(InstallationCancelledPayload payload)
            : base(payload.JobID, payload, "jobId")
        {
        }
    }
}
