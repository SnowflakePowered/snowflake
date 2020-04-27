using HotChocolate.Types;
using Snowflake.Configuration;
using Snowflake.Extensibility;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using Snowflake.Remoting.GraphQL.Model.Configuration;
using Snowflake.Remoting.GraphQL.Model.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Configuration
{
    internal sealed class UpdatePluginConfigurationValuePayload
        : RelayMutationBase
    {
        public List<IConfigurationValue> Values { get; set; }
        public IPlugin Plugin { get; set; }
    }

    internal sealed class UpdatePluginConfigurationValuePayloadType
        : ObjectType<UpdatePluginConfigurationValuePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdatePluginConfigurationValuePayload> descriptor)
        {
            descriptor.Name(nameof(UpdatePluginConfigurationValuePayload))
                .WithClientMutationId();
            descriptor.Field(g => g.Values)
                .Description("The modified configuration values.")
                .Type<NonNullType<ListType<NonNullType<ConfigurationValueType>>>>();
            descriptor.Field(g => g.Plugin)
                .Description("The plugin that uses this configuration.")
                .Type<NonNullType<PluginType>>();
        }
    }
}
