using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Snowflake.Input.Controller;
using Snowflake.Input.Controller.Mapped;
using Snowflake.Input.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Devices.Mapped
{
    public sealed class ControllerElementMappingProfileNodeQueries
        : ObjectTypeExtension<IControllerElementMappingProfile>
    {
        private static readonly string Separator = $":*{Environment.NewLine}";
        protected override void Configure(IObjectTypeDescriptor<IControllerElementMappingProfile> descriptor)
        {
            descriptor.Name("ControllerElementMappingProfile")
                .Interface<NodeType>();

            descriptor.Field("id")
                .Type<IdType>()
                .Resolver(ctx => {
                    var profile = ctx.Parent<IControllerElementMappingProfile>();
                    string concatenatedId = $"{profile.ControllerID}{Separator}" +
                    $"{profile.ProfileName}{Separator}" +
                    $"{profile.DeviceName}{Separator}" +
                    $"{profile.VendorID}{Separator}" +
                    $"{(int)profile.DriverType}";
                    return concatenatedId;
                });
            descriptor.AsNode()
                .NodeResolver<string>((ctx, id) => {
                    string[] parts = id.Split(Separator, 5);
                    if (parts.Length != 5) return null;
                    if (!Int32.TryParse(parts[3], out int vendorId) || !Int32.TryParse(parts[4], out int driverType)) return null;
                    (ControllerId controllerId, string profileName, string deviceName) = (parts[0], parts[1], parts[2]);
                        
                    return ctx.Service<IControllerElementMappingProfileStore>()
                        .GetMappingsAsync(controllerId, (InputDriver)driverType, deviceName, vendorId, profileName);
                });
        }
    }
}
