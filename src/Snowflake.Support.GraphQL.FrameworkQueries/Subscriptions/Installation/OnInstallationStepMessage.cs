using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal class OnInstallationStepMessage
    : SingleIdentifierChannelMessage
    {
        public OnInstallationStepMessage(Guid jobId, InstallationStepPayload payload)
            : base(jobId, payload, "onInstallationStep", nameof(jobId))
        {
        }
    }
}
