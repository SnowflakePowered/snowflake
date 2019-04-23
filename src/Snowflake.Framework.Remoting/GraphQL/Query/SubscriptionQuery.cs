using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GraphQL.Subscription;
using GraphQL.Types;

namespace Snowflake.Framework.Remoting.GraphQL.Query
{
    internal class SubscriptionQuery
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public Type GraphType { get; set; }
        public QueryArguments Arguments { get; set; }
        public Func<ResolveFieldContext<object>, object> Resolver { get; set; }
        public Func<ResolveEventStreamContext<object>, IObservable<object>> Subscriber { get; set; } 
    }
}
