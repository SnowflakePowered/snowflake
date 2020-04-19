using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Queries.Debug
{
    public class SubscriptionDebugQueries
        : ObjectTypeExtension
    {
        static async IAsyncEnumerable<string> HelloWorldAsync()
        {
            yield return "Hello World Sub 1";
            yield return "Hello World Sub 2";
        }

        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Name("Subscription");

            descriptor.Field("helloWorldSubscription")
                .Subscribe(ctx =>
                {
                    return SubscriptionDebugQueries.HelloWorldAsync();
                })
                .Resolver(ctx =>
                {
                    var message = (string)ctx.ContextData["HotChocolate.Execution.EventMessage"];
                    return message;
                })
                .Type<StringType>();
        }
    }
}
