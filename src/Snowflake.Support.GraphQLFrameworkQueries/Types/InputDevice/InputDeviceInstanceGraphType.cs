using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice.Mapped;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.InputDevice
{
    public class InputDeviceInstanceGraphType : ObjectGraphType<IInputDeviceInstance>
    {
        public InputDeviceInstanceGraphType()
        {
            Name = "InputDeviceInstance";
            Description = "An instance of an enumerated input device as exposed by an input driver.";
            Field<InputDriverEnum>("inputDriver",
                description: "The input driver, or input API that enumerated this instance.",
                resolve: context => context.Source.Driver);
            Field<ListGraphType<DeviceCapabilityEnum>>("capabilities",
                description: "The capabilities of the input device instance.",
                resolve: context => context.Source.Capabilities);
            Field<ListGraphType<MappedControllerElementGraphType>>("defaultLayout",
                description: "The default, assumed natural mapping from capability to virtual element without regard for any specific layout.",
                resolve: context => (IEnumerable<MappedControllerElement>)context.Source.DefaultLayout);
            Field<IntGraphType>("enumerationIndex",
                description: "When enumerating devices with a given driver, the index of enumeration for this driver.",
                resolve: context => context.Source.EnumerationIndex);
            Field<IntGraphType>("classEnumerationIndex",
               description: "When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique PID/VID combination," +
                            "if and only if the driver disambiguates between different devices.",
               resolve: context => context.Source.ClassEnumerationIndex);
            Field<IntGraphType>("nameEnumerationIndex",
                description: "When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique product name," +
                            "if and only if the driver disambiguates between different devices.",
                resolve: context => context.Source.NameEnumerationIndex);
            Field<IntGraphType>("productEnumerationIndex",
                description: "When enumerating devices with a given driver, the index of enumeration for this driver," +
                            "with regards to the specific type of device, as determined by unique VID/product name," +
                            "if and only if the driver disambiguates between different devices.",
                resolve: context => context.Source.ProductEnumerationIndex);
        }
    }
}
