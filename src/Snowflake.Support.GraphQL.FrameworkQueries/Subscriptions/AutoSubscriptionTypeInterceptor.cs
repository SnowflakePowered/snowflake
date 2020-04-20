using HotChocolate.Configuration;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions
{
    internal sealed class AutoSubscriptionTypeInterceptor : ITypeInitializationInterceptor
    {
        internal static readonly string AutoSubscriptionContext = "Snowflake.Support.GraphQL.FrameworkQueries.AutoMutationSubscription";
        public bool CanHandle(ITypeSystemObjectContext context)
        {
            return context.Type.Name == "Subscription";
        }

        public void OnAfterCompleteName(ICompletionContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
        }

        public void OnAfterCompleteType(ICompletionContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
        }

        public void OnAfterRegisterDependencies(IInitializationContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
        }

        public void OnBeforeCompleteName(ICompletionContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
        }

        public void OnBeforeCompleteType(ICompletionContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            var def = (ObjectTypeDefinition)definition;

            var autoList = ((List<ObjectFieldDefinition>)context.ContextData[AutoSubscriptionContext]);
            foreach (var mutationDef in autoList)
            {
                string subscriptionName = $"on{mutationDef.Name.Value.ToPascalCase()}";
                ITypeReference mutationType = mutationDef.Type;
                IOutputType type = context.GetType<IOutputType>(mutationType);
                var descriptor = ObjectFieldDescriptor.New(context.DescriptorContext, subscriptionName);
                descriptor
                    .Type(type)
                    .Description($"Subscription for the {mutationDef.Name.Value} mutation.")
                    .Resolver(ctx =>
                    {
                        return ctx.GetEventMessage().Payload;
                    });
                def.Fields.Add(descriptor.CreateDefinition());
            }
        }

        public void OnBeforeRegisterDependencies(IInitializationContext context, DefinitionBase definition, IDictionary<string, object> contextData)
        {
        }
    }
}
