using HotChocolate.Resolvers;
using Snowflake.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL
{
    /// <summary>
    /// Handles Snowflake service access within GraphQL resolvers.
    /// </summary>
    public static class SnowflakeGraphQLServices
    {
        internal static readonly string ServicesNamespace = "Snowflake.Remoting.GraphQL.ServiceContainer";

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
            if (!context.ContextData.TryGetValue(SnowflakeGraphQLServices.ServicesNamespace, out object container)) 
                return null;
            var serviceContainer = (IServiceContainer)container;
            return serviceContainer.Get<T>();
        }
    }
}
