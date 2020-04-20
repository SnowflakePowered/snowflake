using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Queueing
{
    public sealed class QueuableJobInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("QueueableJob")
                .Description("Describes a job that can be queued.");
            descriptor.Field("jobId")
                .Description("The GUID of the job, unique relative to the job context.")
                .Type<NonNullType<UuidType>>();
        }
    }
}
