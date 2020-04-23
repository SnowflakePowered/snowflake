using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay
{
    public static class RelayMutation
    {
        public static IObjectFieldDescriptor UseClientMutationId(this IObjectFieldDescriptor descriptor)
        {
            descriptor.Use(next => async context =>
            {
                await next(context);
                if (context.Argument<object>("input") is RelayMutationBase input && context.Result is RelayMutationBase)
                {
                    ((RelayMutationBase)context.Result).ClientMutationID = input.ClientMutationID;
                }
            });
            return descriptor;
        }

        public static IInputObjectTypeDescriptor<T> WithClientMutationId<T>(this IInputObjectTypeDescriptor<T> descriptor)
            where T : RelayMutationBase
        {
            descriptor.Field(i => i.ClientMutationID)
                .Name("clientMutationId")
                .Type<StringType>();
            return descriptor;
        }

        public static IObjectTypeDescriptor<T> WithClientMutationId<T>(this IObjectTypeDescriptor<T> descriptor)
            where T : RelayMutationBase
        {
            descriptor.Field(i => i.ClientMutationID)
                .Name("clientMutationId")
                .Type<StringType>();
            return descriptor;
        }

        public static IInterfaceTypeDescriptor WithClientMutationId(this IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Field("clientMutationId")
                .Type<StringType>();
            return descriptor;
        }
    }
}
