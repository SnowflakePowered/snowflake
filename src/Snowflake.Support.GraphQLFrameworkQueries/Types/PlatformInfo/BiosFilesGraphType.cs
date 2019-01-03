using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Game;
using Snowflake.Platform;

namespace Snowflake.Support.Remoting.GraphQL.Types.PlatformInfo
{
    internal class BiosFilesGraphType : ObjectGraphType<IBiosFile>
    {
        public BiosFilesGraphType()
        {
            Description = "The BIOS Files that this Platform is known to have.";
            Name = "BiosFiles";
            Field<NonNullGraphType<StringGraphType>>("fileName",
                description: "The file name of this BIOS file.",
                resolve: context => context.Source.FileName);
            Field<NonNullGraphType<StringGraphType>>("md5Hash",
                description: "The MD5 hash of this BIOS file.",
                resolve: context => context.Source.Md5Hash);
        }
    }
}
