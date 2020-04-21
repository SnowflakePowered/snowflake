using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Scraping;
using Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Scraping;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;
using Snowflake.Model.Game;
using Snowflake.Scraping.Extensibility;
using Snowflake.Services;
using System.Linq;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions.Scraping
{
    internal sealed class ScrapingSubscriptions
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name("Subscription");

            descriptor.Field("onScrapeContextStep")
                .Type<NonNullType<ScrapeContextStepPayloadType>>()
                .Argument("jobId", arg => arg.Type<NonNullType<UuidType>>())
                .Resolver(ctx =>
                {
                    var message = ctx.GetEventMessage<OnScrapeContextStepMessage>();
                    return message.Payload;
                });
        }
    }
}
