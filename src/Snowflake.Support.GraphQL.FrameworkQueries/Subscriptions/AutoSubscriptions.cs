using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Subscriptions
{
    public sealed class AutoSubscriptions
        : ObjectTypeExtension
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor
                .Name("Subscription")
               
                .Extend()
                
                .OnBeforeNaming((context, def) =>
                {
                    var descriptor = ObjectFieldDescriptor.New(context.DescriptorContext, "testBeforeNaming");
                    descriptor.Type<StringType>();

                    def.Fields.Add(descriptor.CreateDefinition());
                });
            descriptor
                .Name("Subscription")
                .Extend()
                .OnBeforeCompletion((context, def) =>
                {
                    var descriptor = ObjectFieldDescriptor.New(context.DescriptorContext, "testBeforeCompletion");
                    descriptor.Type<StringType>();

                    def.Fields.Add(descriptor.CreateDefinition());
                });
        }
    }
}
