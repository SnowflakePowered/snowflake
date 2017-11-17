using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    internal class BiosFilesType: ObjectGraphType<IGrouping<string, string>>
    {
        public BiosFilesType()
        {
            Name = "BiosFiles";
            Field<NonNullGraphType<StringGraphType>>("fileName",
                description: "The file name of this BIOS file.",
                resolve: context => context.Source.Key
            );
            Field<ListGraphType<StringGraphType>>("md5Hash",
                description: "The MD5 hash of this BIOS file.",
                resolve: context => context.Source.ToList()
           );
        }
    }
}
