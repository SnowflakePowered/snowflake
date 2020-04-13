using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem;
using Snowflake.Framework.Remoting.GraphQL.Model.Filesystem.Contextual;
using Snowflake.Framework.Remoting.GraphQL.Model.Records;
using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Zio;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game.LibraryExtensions
{
    public sealed class GameFileExtensionType
         : ObjectType<IGameFileExtension>
    {
        protected override void Configure(IObjectTypeDescriptor<IGameFileExtension> descriptor)
        {
            descriptor.Name("GameFiles")
                .Description("Files and file data relating to the game.");
            descriptor.Field("fileRecords")
                .Resolver(g => g.Parent<IGameFileExtension>()
                .GetFileRecords())
                .Type<ListType<FileRecordType>>();
            descriptor.Field("fs")
                .Argument("directoryPath", a => a.Type<DirectoryPathType>())
                .Type<ContextualDirectoryContents>()
                .Resolver(context =>
                {
                    if (context.ArgumentKind("directoryPath") == ValueKind.Null) return context.Parent<IGameFileExtension>().Root;
                    var path = context.Argument<UPath>("directoryPath");
                    return context.Parent<IGameFileExtension>().Root.OpenDirectory(path.FullName);
                });

        }
    }
}
