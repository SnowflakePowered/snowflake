using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.RelayMutations
{
    public static class RelayMutation
    {
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

        public static void Reconcile(RelayMutationBase input, RelayMutationBase payload)
        {
            payload.ClientMutationID = input.ClientMutationID;
        }
    }
}
