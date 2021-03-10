using System;
using System.Collections.Generic;
using System.Text;
using HotChocolate.Types;
using Snowflake.Input.Controller;
using Snowflake.Input.Device;

namespace Snowflake.Remoting.GraphQL.Model.Device
{
    public sealed class InputDeviceInstanceType : ObjectType<IInputDeviceInstance>
    {
        protected override void Configure(IObjectTypeDescriptor<IInputDeviceInstance> descriptor)
        {
            //Field<ListGraphType<MappedControllerElementGraphType>>("defaultLayout",
            //   description: "The default, assumed natural mapping from capability to virtual element without regard for any specific layout.",
            //   resolve: context => context.Source.DefaultLayout);

            descriptor.Name("InputDeviceInstance")
                .Description("An instance of an enumerated input device as exposed by an input driver.");
            descriptor.Field(c => c.Driver)
                .Name("inputDriver")
                .Description("The input driver, or input API that enumerated this instance.")
                .Type<NonNullType<InputDriverEnum>>();
            descriptor.Field(c => c.EnumerationIndex)
                .Description("When enumerating devices with a given driver, the index of enumeration for this driver.");
            descriptor.Field(c => c.ClassEnumerationIndex)
                .Description("When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique PID/VID combination," +
                            "if and only if the driver disambiguates between different devices.");
            descriptor.Field(c => c.NameEnumerationIndex)
                .Description("When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique product name," +
                            "if and only if the driver disambiguates between different devices.");
            descriptor.Field(c => c.ProductEnumerationIndex)
                .Description("When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique VID/product name," +
                            "if and only if the driver disambiguates between different devices.");
            descriptor.Field(c => c.CapabilityLabels)
                .Description("Friendly labels for the device capabilities of this instance")
                .Type<DeviceCapabilityLabelsType>();
            descriptor.Field(c => c.Capabilities)
                .Description("The capabilities of the input device instance.")
                .Type<NonNullType<ListType<NonNullType<DeviceCapabilityEnum>>>>();
        }
    }
}
