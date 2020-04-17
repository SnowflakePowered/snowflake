using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Game;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.PlatformInfo
{
    internal class SystemFileGraphType : ObjectGraphType<ISystemFile>
    {
        public SystemFileGraphType()
        {
            Description = "A BIOS or System File that is required for an emulator to run a game.";
            Name = "SystemFile";
            Field<NonNullGraphType<StringGraphType>>("fileName",
                description: "The file name of this BIOS file.",
                resolve: context => context.Source.FileName);
            Field<NonNullGraphType<StringGraphType>>("md5Hash",
                description: "The MD5 hash of this BIOS file.",
                resolve: context => context.Source.Md5Hash);
        }
    }
}
