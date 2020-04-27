using HotChocolate.Language;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions
{
    internal abstract class SingleIdentifierChannelMessage
        : EventMessage
    {
        public SingleIdentifierChannelMessage(Guid identifier, object payload, string subscriptionName, string identifierName)
            : base(CreateEventDescription(identifier, identifierName, subscriptionName), payload)
        { }

        private static EventDescription CreateEventDescription(Guid identifier, string identifierName, string subscriptionName)
        {
            return new EventDescription(subscriptionName,
                new ArgumentNode(identifierName, new StringValueNode(identifier.ToString("N"))));
        }
    }
}
