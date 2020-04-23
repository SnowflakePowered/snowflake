using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal class OnInstallationStepMessage
    : EventMessage
    {
        public OnInstallationStepMessage(Guid jobId, InstallationStepPayload payload)
            : base(CreateEventDescription(jobId), payload)
        {
        }

        private static EventDescription CreateEventDescription(Guid jobId)
        {
            return new EventDescription("onInstallationStep",
                new ArgumentNode("jobId", new StringValueNode(jobId.ToString("N"))));
        }
    }
}
