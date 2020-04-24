using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Installation.Tasks
{
    public sealed class TaskResultErrorUnionType<TSuccessType>
        : UnionType where TSuccessType : ObjectType
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor
                .Type<TSuccessType>()
                .Type<InstallationExceptionType>();

            descriptor
                .Extend()
                .OnBeforeNaming((context, definition) =>
                {
                    var successType = context.GetType<TSuccessType>(definition.Types[0]);
                    definition.Name = $"{successType.Name}TaskResultValue";
                })
                .DependsOn<TSuccessType>()
                .DependsOn<InstallationExceptionType>();
        }
    }
}
