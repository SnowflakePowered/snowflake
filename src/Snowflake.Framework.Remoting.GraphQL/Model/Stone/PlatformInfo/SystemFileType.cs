using HotChocolate.Types;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Remoting.GraphQL.Model.Stone.PlatformInfo
{
    public sealed class SystemFileType
        : ObjectType<ISystemFile>
    {
        protected override void Configure(IObjectTypeDescriptor<ISystemFile> descriptor)
        {
            descriptor.Name("SystemFile")
                .Description("A BIOS or System File that is required for an emulator to run a game.");
            descriptor.Field(c => c.FileName)
                .Description("The file name of this BIOS file.")
                .Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Md5Hash)
              .Description("The MD5 hash of this BIOS file.")
              .Type<NonNullType<StringType>>();
        }
    }
}
