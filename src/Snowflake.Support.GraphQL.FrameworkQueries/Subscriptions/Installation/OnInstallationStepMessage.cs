using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Installation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Installation
{
    internal class OnInstallationStepMessage
    : EventMessage<Guid>
    {
        public OnInstallationStepMessage(InstallationStepPayload payload)
            : base(payload.JobID, payload)
        {
        }
    }
}
