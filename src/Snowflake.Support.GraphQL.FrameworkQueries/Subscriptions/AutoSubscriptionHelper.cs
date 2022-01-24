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

        public static IObjectFieldDescriptor UseAutoSubscription(this IObjectFieldDescriptor descriptor, string subscriptionName)
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
                await context.SendSimpleSubscription(subscriptionName, context.Result);
            });
            return descriptor;
        }

        public static IObjectFieldDescriptor SubscribeToTopic<TArg, T>(this IObjectFieldDescriptor descriptor, string argumentName)
            where T: EventMessage<TArg>
        {
            descriptor.SubscribeToTopic<(Type, TArg), object>
                (ctx => (typeof(T), ctx.ArgumentValue<TArg>(argumentName)));
            return descriptor;
        }

        public static ValueTask SendEventMessage<T>(this IResolverContext context, EventMessage<T> message)
        {
            var eventSender = context.Service<ITopicEventSender>();
            return eventSender.SendAsync((message.GetType(), message.Topic), message.Payload);
        }

        public static async ValueTask<T> SendSimpleSubscription<T>(this IResolverContext context, string subscriptionName, T payload)
        {
            var eventSender = context.Service<ITopicEventSender>();
            await eventSender.SendAsync(subscriptionName, payload);
            return payload;
        }

        internal static ValueTask<T> SendSimpleSubscription<T>(this IResolverContext context, T payload)
        {
            string subscriptionName = $"on{context.Selection.Field.Name.Value.ToPascalCase()}";
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
