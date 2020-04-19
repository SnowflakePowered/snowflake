using HotChocolate.Types;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerElementCollectionType
        : ObjectType<IControllerElementCollection>
    {
        protected override void Configure(IObjectTypeDescriptor<IControllerElementCollection> descriptor)
        {
            descriptor.Name("ControllerElementCollection")
                .Description("A collection of Controller Element definitions that describe a Controller Layout.");
            descriptor.Field("elements")
                .Description("A list of element definitions.")
                .Resolver(ctx => ctx.Parent<IControllerElementCollection>())
                .Type<NonNullType<ListType<NonNullType<ControllerElementInfoElementType>>>>();
            //descriptor.BindFieldsImplicitly()
            //    .Ignore(c => c[default]);
        }
    }
}
