using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions
{
    public sealed class SimpleEventMessage<T>
        : EventMessage
    {
        public SimpleEventMessage(string subscriptionName, T payload)
            : base(new EventDescription(subscriptionName), payload)
        {
        }
    }
}
