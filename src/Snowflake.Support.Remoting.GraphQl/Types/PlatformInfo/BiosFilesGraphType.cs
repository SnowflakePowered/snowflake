using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    internal class BiosFilesGraphType : ObjectGraphType<IGrouping<string, string>>
    {
        public BiosFilesGraphType()
        {
            Description = "The BIOS Files that this Platform is known to have.";
            Name = "BiosFiles";
            Field<NonNullGraphType<StringGraphType>>("fileName",
                description: "The file name of this BIOS file.",
                resolve: context => context.Source.Key);
            Field<ListGraphType<StringGraphType>>("md5Hash",
                description: "The MD5 hash of this BIOS file.",
                resolve: context => context.Source.ToList());
        }
    }
}
