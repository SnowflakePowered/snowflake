using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;
using Snowflake.Installation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation.Tasks
{
    public sealed class TaskResultType<T, TType>
        : ObjectType<TaskResult<T>> where TType : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor<TaskResult<T>> descriptor)
        {
            descriptor
                .Interface<TaskResultTypeInterface>()
                .Extend()
                .OnBeforeNaming((completionContext, definition) =>
                {
                    var valueTypeReference = definition.Fields.First(f => f.Name == "value").Type;
                    var unionType = completionContext.GetType<TaskResultErrorUnionType<TType>>(valueTypeReference);
                    string unionTypeName = unionType.Name.ToString();
                    // Nasty hack that relies on the union value being TaskResultValue.
                    // I would have liked to do this properly but types are not populated early enough
                    // for the union type. 
                    var dependentName = unionTypeName.Substring(0, unionTypeName.Length - "TaskResultValue".Length);
                    definition.Name = $"{dependentName}TaskResult";
                    definition.Description = $"Describes a task result that yields {dependentName}";
                })
                .DependsOn<TaskResultErrorUnionType<TType>>(true)
                .DependsOn<TType>(true);

            descriptor.Field("description")
                .Description("Describes this task result in a human-friendly way.")
                .Resolver(async ctx => await (ctx.Parent<TaskResult<T>>().Description))
                .Type<StringType>();
            descriptor.Field(t => t.Name)
                .Description("The name of this task result.")
                .Type<StringType>();
            descriptor.Field("errored")
                .Description("Whether or not this task has errored.")
                .Resolver(ctx => ctx.Parent<TaskResult<T>>().Error != null)
                .Type<NonNullType<BooleanType>>();
            descriptor.Field("value")
                .Resolver(async ctx =>
                {
                    var taskResult = ctx.Parent<TaskResult<T>>();
                    // lol if err != nil
                    if (taskResult.Error != null) return taskResult.Error;
                    return await taskResult;
                })
                .Type<TaskResultErrorUnionType<TType>>();
        }
    }
}
