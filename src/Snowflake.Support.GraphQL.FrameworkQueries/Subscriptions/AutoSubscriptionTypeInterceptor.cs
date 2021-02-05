using HotChocolate;
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

        public bool TriggerAggregations => true;

        public bool CanHandle(ITypeSystemObjectContext context)
        {
            return context.Type.Name == "Subscription";
        }

        public void OnAfterCompleteName(ITypeCompletionContext completionContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            
        }

        public void OnAfterCompleteType(ITypeCompletionContext completionContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            
        }

        public void OnAfterInitialize(ITypeDiscoveryContext discoveryContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            
        }

        public void OnAfterRegisterDependencies(ITypeDiscoveryContext discoveryContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
            
        }

        public void OnBeforeCompleteName(ITypeCompletionContext completionContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
           
        }

        public void OnBeforeCompleteType(ITypeCompletionContext context, DefinitionBase definition, IDictionary<string, object> contextData)
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
                    .SubscribeToTopic<object>(subscriptionName)
                    .Resolve(ctx =>
                    {
                        return ctx.GetEventMessage<object>();
                    });
                def.Fields.Add(descriptor.CreateDefinition());
            }
            def.Description += 
@"

Snowflake provides two types of definition in its framework queries:
  * `onVerbObject`
  * `onObjectVerb(Uuid!)`

`onVerbObject` subscriptions are global, and are broadcast whenever the corresponding mutation occurs. These can be used to subscribe to mutations that are triggered by the client.
`onObjectVerb(Uuid!)` subscriptions are primarily used by scraping, installation, and orchestration mutations. These are used to subscribe to events happening on a specific long-existing object that may or may not be the result of a client request.

There is some subtlely in the different types of subscriptions that one should be aware about.

For example, `onStopEmulation` is broadcast when the `stopEmulation` mutation responds to some client. However, `onEmulationStop` is broadcast when the emulation process exits. Hence, `onStopEmulation` may never be broadcast, even if `onEmulationStop` was.

In most cases, it is more useful to subscribe to the `onObjectVerb` subscription for an object whenever feasible.
";
        }

        public void OnBeforeInitialize(ITypeDiscoveryContext discoveryContext)
        {
            
        }

        public void OnBeforeRegisterDependencies(ITypeDiscoveryContext discoveryContext, DefinitionBase definition, IDictionary<string, object> contextData)
        {
           
        }

        public void OnTypesCompleted(IReadOnlyCollection<ITypeCompletionContext> completionContexts)
        {
           
        }

        public void OnTypesCompletedName(IReadOnlyCollection<ITypeCompletionContext> completionContexts)
        {
           
        }

        public void OnTypesInitialized(IReadOnlyCollection<ITypeDiscoveryContext> discoveryContexts)
        {
            
        }
    }
}
