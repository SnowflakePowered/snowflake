using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.FrameworkQueries.Mutations.Relay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQL.FrameworkQueries.Mutations.Orchestration
{
    public sealed class EmulationInstanceInput
        : RelayMutationBase
    {
        public Guid InstanceID { get; set; }
    }
    internal sealed class EmulationInstanceInputType
        : InputObjectType<EmulationInstanceInput>
    {
        protected override void Configure(IInputObjectTypeDescriptor<EmulationInstanceInput> descriptor)
        {
            descriptor.Name(nameof(EmulationInstanceInput))
                .WithClientMutationId();
            
            descriptor.Field(i => i.InstanceID)
                .Name("instanceId")
                .Description("The GUID of the emulation instance to use as a handle to modify the instance.")
                .Type<NonNullType<UuidType>>();
        }
    }
}
