using HotChocolate.Types;
using Snowflake.Orchestration.Saving;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Saving
{
    public sealed class SaveManagementStrategyEnum
        : EnumType<SaveManagementStrategy>
    {
        protected override void Configure(IEnumTypeDescriptor<SaveManagementStrategy> descriptor)
        {
            descriptor.Name("SaveManagementStrategy")
                .Description("Strategies used to manage save data.")
                .BindValues(BindingBehavior.Implicit);
        }
    }
}
