﻿using HotChocolate.Types;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo
{
    public sealed class PlatformInfoType
        : ObjectType<IPlatformInfo>
    {
        protected override void Configure(IObjectTypeDescriptor<IPlatformInfo> descriptor)
        {
            descriptor.Name("PlatformInfo").Description("A Stone Platform description.");
            descriptor.Field(p => p.PlatformID)
                .Name("platformId")
                .Type<NonNullType<PlatformIdType>>()
                .Description("The Stone Platform ID of this platform.");
            descriptor.Field(p => p.FriendlyName).Description("The human readable name of this platform.");
            descriptor.Field(p => p.MaximumInputs).Description("The maximum inputs this platform can have.");
            descriptor.Field(p => p.FileTypes).Description("Known Stone mimetypes of ROMs for this platform.")
                .Type<NonNullType<ListType<NonNullType<FileTypeType>>>>();
            descriptor.Field(p => p.Metadata).Description("Stone metadata for this platform.")
               .Type<NonNullType<ListType<NonNullType<StoneMetadataType>>>>();
            descriptor.Field(p => p.BiosFiles).Description("Known BIOS or system files for this platform.")
               .Type<NonNullType<ListType<NonNullType<SystemFileType>>>>();
        }
    }
}
