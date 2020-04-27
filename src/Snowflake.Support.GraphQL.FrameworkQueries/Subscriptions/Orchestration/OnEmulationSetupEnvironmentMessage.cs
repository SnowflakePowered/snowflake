using HotChocolate.Language;
using HotChocolate.Subscriptions;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Orchestration
{
    internal class OnEmulationSetupEnvironmentMessage
        : SingleIdentifierChannelMessage
    {
        public OnEmulationSetupEnvironmentMessage(EmulationInstancePayload payload)
            : base(payload.InstanceID, payload, "onEmulationSetupEnvironment", "instanceId")
        {
        }
    }
}
