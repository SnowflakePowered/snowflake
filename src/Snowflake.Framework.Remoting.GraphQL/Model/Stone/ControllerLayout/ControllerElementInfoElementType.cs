using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate.Types;
using Snowflake.Input.Controller;

namespace Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerElementInfoElementType : ObjectType<KeyValuePair<ControllerElement, IControllerElementInfo>>
    {
        protected override void Configure(IObjectTypeDescriptor<KeyValuePair<ControllerElement, IControllerElementInfo>> descriptor)
        {
            descriptor.Name("ControllerElementInfoElement")
                .Description("Defines a single element/capability of a controller layout by the semantic element, and its type.");
            descriptor.Field("type")
                .Resolve(ctx => ctx.Parent<KeyValuePair<ControllerElement, IControllerElementInfo>>().Value.Type)
                .Type<NonNullType<ControllerElementTypeEnum>>()
                .Description("The semantic type of this controller element.");
            descriptor.Field("label")
                .Resolve(ctx => ctx.Parent<KeyValuePair<ControllerElement, IControllerElementInfo>>().Value.Label)
                .Description("The human-readable label of this controller element.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Key)
                .Name("element")
                .Description("The semantic element represented by this definition.")
                .Type<NonNullType<ControllerElementEnum>>();
        }
    }
}
