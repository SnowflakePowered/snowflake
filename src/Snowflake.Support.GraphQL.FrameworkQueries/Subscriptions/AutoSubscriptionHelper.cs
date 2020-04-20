using HotChocolate.Resolvers;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors.Definitions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions
{
    internal static class AutoSubscriptionHelper
    {
        public static IObjectFieldDescriptor UseAutoSubscription(this IObjectFieldDescriptor descriptor)
        {
            descriptor
                .Extend()
                .OnBeforeNaming((configure, defn) =>
                {
                    if (!configure.ContextData.ContainsKey(AutoSubscriptionTypeInterceptor.AutoSubscriptionContext))
                    {
                        configure.ContextData[AutoSubscriptionTypeInterceptor.AutoSubscriptionContext] = new List<ObjectFieldDefinition>();
                    }
                    ((List<ObjectFieldDefinition>)configure.ContextData[AutoSubscriptionTypeInterceptor.AutoSubscriptionContext]).Add(defn);
                });
            descriptor.Use(next => async context =>
            {
                await next(context);
                await context.SendSimpleSubscription(context.Result);
            });
            return descriptor;
        }

        public static T GetEventMessage<T>(this IResolverContext context) => (T)context.ContextData["HotChocolate.Execution.EventMessage"];
        public static EventMessage GetEventMessage(this IResolverContext context) => (EventMessage)context.ContextData["HotChocolate.Execution.EventMessage"];

        public static ValueTask SendEventMessage(this IResolverContext context, IEventMessage message)
        {
            var eventSender = context.Service<IEventSender>();
            return eventSender.SendAsync(message);
        }

        public static async ValueTask<T> SendSimpleSubscription<T>(this IResolverContext context, string subscriptionName, T payload)
        {
            await context.SendEventMessage(new SimpleEventMessage<T>(subscriptionName, payload));
            return payload;
        }

        internal static ValueTask<T> SendSimpleSubscription<T>(this IResolverContext context, T payload)
        {
            string subscriptionName = $"on{context.Field.Name.Value.ToPascalCase()}";
            return context.SendSimpleSubscription(subscriptionName, payload);
        }

        /// <summary>
        /// Returns a pascal case version of the string.
        /// </summary>
        /// <param name="s">The source string.</param>
        /// <returns>System.String.</returns>
        internal static string ToPascalCase(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return string.Empty;
            }

            var newFirstLetter = char.ToUpperInvariant(s[0]);
            if (newFirstLetter == s[0])
                return s;

            return newFirstLetter + s.Substring(1);
        }
    }
}
