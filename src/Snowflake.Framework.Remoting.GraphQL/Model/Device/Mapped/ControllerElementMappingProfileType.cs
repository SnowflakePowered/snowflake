using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Stone.ControllerLayout;
using Snowflake.Input.Controller;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Device.Mapped
{
    public sealed class ControllerElementMappingProfileType
        : ObjectType<IControllerElementMappingProfile>
    {
        protected override void Configure(IObjectTypeDescriptor<IControllerElementMappingProfile> descriptor)
        {
            descriptor.Name("ControllerElementMappingProfile")
                .Description("A collection of controller element to device capability mappings that describes a " +
                "mapping profile from an input device to an emulated virtual device.");
            descriptor.Field(c => c.ProfileGuid)
                .Name("profileId")
                .Description("The Profile GUID of this mapping profile.")
                .Type<ControllerIdType>();
            descriptor.Field(c => c.ControllerID)
                .Name("controllerId")
                .Description("The Stone Controller ID of the emulated controller this collection maps to.")
                .Type<ControllerIdType>();
            descriptor.Field(c => c.DeviceName)
                .Description("The name of the input device this profile maps capabilities from. " +
                "Together with the driver type and vendor ID, uniquely identifies the input device instances for which this profile is valid.")
                .Type<StringType>();
            descriptor.Field(c => c.VendorID)
                .Name("vendorId")
                .Description("The vendor ID of the input device this profile maps capabilities from. " +
                "Together with the device name and drive type, uniquely identifies the input device instances for which this profile is valid.")
               .Type<IntType>();
            descriptor.Field(c => c.DriverType)
               .Description("The driver that produced the input device instance this profile maps capabilities from. " +
                "Together with the device name and vendor ID, uniquely identifies the input device instances for which this profile is valid.")
               .Type<IntType>();
            descriptor.Field("mappings")
                .Resolve(c => c.Parent<IControllerElementMappingProfile>())
                .Description("The set of mappings that map each capability from the real device to the element on the emulated device.")
                .Type<NonNullType<ListType<NonNullType<ControllerElementMappingType>>>>();
        }
    }
}
