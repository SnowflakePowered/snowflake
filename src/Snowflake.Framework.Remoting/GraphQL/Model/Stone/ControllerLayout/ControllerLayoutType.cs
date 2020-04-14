using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Stone.PlatformInfo;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Stone.ControllerLayout
{
    public sealed class ControllerLayoutType
        : ObjectType<IControllerLayout>
    {
        protected override void Configure(IObjectTypeDescriptor<IControllerLayout> descriptor)
        {
            descriptor.Name("ControllerLayout")
                .Description("A Stone Controller Layout. " +
                "Describes the layout of a known controller by a collection of element definition.");
            descriptor.Field(c => c.LayoutID)
                .Description("The Stone ID of the Controller Layout.")
                .Type<NonNullType<ControllerIdType>>();
            descriptor.Field(c => c.FriendlyName)
                .Description("The human-readable friendly name of this layout.");
            descriptor.Field(c => c.Platforms)
                .Description("The list of platforms that supports this controller, represented as Stone Platform IDs.")
                .Type<NonNullType<ListType<NonNullType<PlatformIdType>>>>();
            descriptor.Field(c => c.Layout)
                .Description("The layout definition of the controller.")
                .Type<NonNullType<ControllerElementCollectionType>>();
        }
    }
}
