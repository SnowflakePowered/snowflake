using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class UpdateGameConfigurationValueInput
        : RelayMutationBase
    {
        public List<ConfigurationValueInput> Values { get; set; }
    }

    internal sealed class UpdateGameConfigurationValueInputType
        : InputObjectType<UpdateGameConfigurationValueInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdateGameConfigurationValueInput> descriptor)
        {
            descriptor.Name(nameof(UpdateGameConfigurationValueInput))
                .WithClientMutationId();
            descriptor.Field(g => g.Values)
                .Type<NonNullType<ListType<NonNullType<ConfigurationValueInputType>>>>();
        }
    }
}
