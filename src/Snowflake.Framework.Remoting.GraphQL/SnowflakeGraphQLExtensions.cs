using HotChocolate.Resolvers;
using HotChocolate.Types;
using Snowflake.Extensibility.Queueing;
using Snowflake.Model.Game;
using Snowflake.Remoting.GraphQL.Model.Queueing;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL
{
    /// <summary>
    /// Handles Snowflake service access within GraphQL resolvers.
    /// </summary>
    public static class SnowflakeGraphQLExtensions
    {
        internal static readonly string ServicesNamespace = "Snowflake.Remoting.GraphQL.ServiceContainer";

        /// <summary>
        /// Extends the root query type.
        /// </summary>
        public static IObjectTypeDescriptor ExtendQuery(this IObjectTypeDescriptor descriptor) => descriptor.Name("Query");

        /// <summary>
        /// Extends the root mutation type.
        /// </summary>
        public static IObjectTypeDescriptor ExtendMutation(this IObjectTypeDescriptor descriptor) => descriptor.Name("Mutation");

        /// <summary>
        /// Extends the root subscription type
        /// </summary>
        public static IObjectTypeDescriptor ExtendSubscription(this IObjectTypeDescriptor descriptor) => descriptor.Name("Subscription");

        /// <summary>
        /// Extends the Game query type.
        /// </summary>
        public static IObjectTypeDescriptor<IGame> ExtendGame(this IObjectTypeDescriptor<IGame> descriptor) => descriptor.Name("Game");

        /// <summary>
        /// Retrieves a service from the Snowflake services container 
        /// within a GraphQL resolver context.
        /// </summary>
        /// <typeparam name="T">The type of service.</typeparam>
        /// <param name="context">The GraphQL resolver context.</param>
        /// <returns>The service instance, if it exists.</returns>
        public static T SnowflakeService<T>(this IResolverContext context)
            where T : class
        {
            if (!context.ContextData.TryGetValue(SnowflakeGraphQLExtensions.ServicesNamespace, out object container)) 
                return null;
            var serviceContainer = (IServiceContainer)container;
            return serviceContainer.Get<T>();
        }

        /// <summary>
        /// Implements fields and interfaces for AsyncJobQueue object types.
        /// The job field must still be manually specified by the client.
        /// </summary>
        /// <typeparam name="T">The type of the job queue.</typeparam>
        /// <param name="descriptor">The GraphQL Type descriptor.</param>
        /// <returns>The GraphQL type descriptor.</returns>
        public static IObjectTypeDescriptor<T> UseJobQueue<T>(this IObjectTypeDescriptor<T> descriptor)
            where T : class, IAsyncJobQueue
        {
            descriptor
                .Implements<JobQueueInterface>();

            descriptor.Field(s => s.GetActiveJobs())
                .Name("activeJobIds")
                .Description("The jobs currently active in the scraping queue.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
            descriptor.Field(s => s.GetQueuedJobs())
                .Name("queuedJobIds")
                .Description("The jobs currently in the scraping queue.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
            descriptor.Field(s => s.GetZombieJobs())
                .Name("zombieJobIds")
                .Description("The jobs that are still in the scraping queue, but no longer has items to process.")
                .Type<NonNullType<ListType<NonNullType<UuidType>>>>();
            return descriptor;
        }
    }
}
