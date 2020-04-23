using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation
{
    public sealed class TaskResultTypeInterface
        : InterfaceType
    {
        protected override void Configure(IInterfaceTypeDescriptor descriptor)
        {
            descriptor.Name("TaskResult")
                .Description("Describes a task result that yields some value.");
            descriptor.Field("description")
               .Description("Describes this task result in a human-friendly way.")
               .Type<NonNullType<StringType>>();
            descriptor.Field("name")
                .Description("The name of this task result.")
                .Type<NonNullType<StringType>>();
        }
    }
}
