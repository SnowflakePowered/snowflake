using System;
using System.Collections.Generic;
using System.Text;
using GraphQL;
using GraphQL.Types;
using Snowflake.Model.Game.LibraryExtensions;

namespace Snowflake.Support.GraphQLFrameworkQueries.Types.Model
{
    public class GameFileExtensionGraphType : ObjectGraphType<IGameFileExtension>
    {
        public GameFileExtensionGraphType()
        {
            Name = "GameFiles";
            Description = "The files of a game";

            Field<ListGraphType<FileRecordGraphType>>("fileRecords",
                description: "The files for which have metadata and are installed, not all files.",
                resolve: context => context.Source.GetFileRecords());

            Field<ListGraphType<FileGraphType>>("programFiles",
                description: "All files inside the program files folder for this game.",
                arguments: new QueryArguments(new QueryArgument<BooleanGraphType>
                    {Name = "recursive", DefaultValue = false,}),
                resolve: context => context.GetArgument("recursive", false)
                    ? context.Source.ProgramRoot.EnumerateFilesRecursive()
                    : context.Source.ProgramRoot.EnumerateFiles());
        }
    }
}
