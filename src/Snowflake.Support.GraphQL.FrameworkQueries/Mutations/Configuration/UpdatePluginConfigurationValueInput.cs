using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class UpdatePluginConfigurationValueInput
        : RelayMutationBase
    {
        public string Plugin { get; set; }
        public List<ConfigurationValueInput> Values { get; set; }
    }

    internal sealed class UpdatePluginConfigurationValueInputType
        : InputObjectType<UpdatePluginConfigurationValueInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<UpdatePluginConfigurationValueInput> descriptor)
        {
            descriptor.Name(nameof(UpdatePluginConfigurationValueInput))
                .WithClientMutationId();
            descriptor.Field(g => g.Plugin)
                .Type<NonNullType<StringType>>();
            descriptor.Field(g => g.Values)
                .Type<NonNullType<ListType<NonNullType<ConfigurationValueInputType>>>>();
        }
    }
}
