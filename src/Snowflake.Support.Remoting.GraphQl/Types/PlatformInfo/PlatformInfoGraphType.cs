using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Builders;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using Snowflake.Platform;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    public class PlatformInfoGraphType : ObjectGraphType<IPlatformInfo>
    {
        public PlatformInfoGraphType()
        {
            Name = "PlatformInfo";
            Description = "A Stone Platform Definition.";
            Field<StringGraphType>("platformId", description: "The Stone PlatformID.",
                resolve: context => context.Source.PlatformId.ToString());
            Field(p => p.FriendlyName).Description("The human readable name of the platform.");
            Field(p => p.MaximumInputs).Description("The maximum inputs this platform can have.");
            Field<ListGraphType<FileTypeGraphType>>("fileType",
                resolve: context =>
                    context.Source.FileTypes.Select(p => new FileType() {Extension = p.Key, Mime = p.Value}));
            Field<ListGraphType<MetadataGraphType>>("metadata", resolve: context => context.Source.Metadata.ToList());
            Field<ListGraphType<BiosFilesGraphType>>("biosFiles",
                resolve: context => context.Source.BiosFiles.ToList());
        }
    }
}
