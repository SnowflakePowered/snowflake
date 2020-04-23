using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Snowflake.Installation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation
{
    public sealed class TaskResultType<T, TType>
        : ObjectType<TaskResult<T>> where TType : class, INamedOutputType
    {
        protected override void Configure(IObjectTypeDescriptor<TaskResult<T>> descriptor)
        {
            descriptor
                .Interface<TaskResultTypeInterface>()
                .Extend()
                .OnBeforeNaming((completionContext, definition) =>
                {
                    var valueTypeReference = definition.Fields.First(f => f.Name == "value").Type;
                    var dependentName = completionContext.GetType<TType>(valueTypeReference).Name;
                    definition.Name = $"{typeof(T).Name}_{dependentName}TaskResult";
                    definition.Description = $"Describes a task result that yields {dependentName}";
                })
                .DependsOn<TType>();

            descriptor.Field("description")
                .Description("Describes this task result in a human-friendly way.")
                .Resolver(async ctx => await (ctx.Parent<TaskResult<T>>().Description))
                .Type<StringType>();
            descriptor.Field(t => t.Name)
                .Description("The name of this task result.")
                .Type<StringType>();
            descriptor.Field("value")
                .Resolver(async ctx => (await ctx.Parent<TaskResult<T>>()))
                .Type<TType>();
        }
    }
}
