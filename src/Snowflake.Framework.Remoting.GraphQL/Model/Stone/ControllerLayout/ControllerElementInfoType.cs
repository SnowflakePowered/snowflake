using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate.Types;
using Snowflake.Input.Controller;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerElementInfoType : ObjectType<IControllerElementInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<IControllerElementInfo> descriptor)
        {
            descriptor.Name("ControllerElementInfo")
                .Description("Defines a single element/capability of a controller layout by the semantic element, and its type.");
            descriptor.Field(c => c.Type)
                .Type<ControllerElementTypeEnum>()
                .Description("The semantic type of this controller element.");
            descriptor.Field(c => c.Label)
                .Description("The human-readable label of this controller element.");
        }
    }
}
