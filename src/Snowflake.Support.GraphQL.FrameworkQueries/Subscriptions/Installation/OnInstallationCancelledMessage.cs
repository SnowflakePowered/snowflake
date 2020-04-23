using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal class OnInstallationCancelledMessage
    : EventMessage
    {
        public OnInstallationCancelledMessage(Guid jobId, InstallationCancelledPayload payload)
            : base(CreateEventDescription(jobId), payload)
        {
        }

        private static EventDescription CreateEventDescription(Guid jobId)
        {
            return new EventDescription("onInstallationCancelled",
                new ArgumentNode("jobId", new StringValueNode(jobId.ToString("N"))));
        }
    }
}
