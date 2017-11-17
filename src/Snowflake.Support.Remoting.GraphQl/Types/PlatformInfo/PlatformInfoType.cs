using GraphQL.Builders;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using Snowflake.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Support.Remoting.GraphQl.Types.PlatformInfo
{
    public class PlatformInfoType : ObjectGraphType<IPlatformInfo>
    {
        public PlatformInfoType()
        {
            Name = "PlatformInfo";
            Description = "A Stone Platform Definition.";
            Field(p => p.PlatformID).Description("The Stone PlatformID.");
            Field(p => p.FriendlyName).Description("The human readable name of the platform.");
            Field(p => p.MaximumInputs).Description("The maximum inputs this platform can have.");
            Field<ListGraphType<FileTypeType>>("fileType", resolve: context => context.Source.FileTypes.Select(p => new FileType() { Extension = p.Key, Mime = p.Value }));
            Field<ListGraphType<MetadataType>>("metadata", resolve: context => context.Source.Metadata.ToList());
            Field<ListGraphType<BiosFilesType>>("biosFiles", resolve: context => context.Source.BiosFiles.ToList());
        }
    }
}
