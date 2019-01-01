using System;
using System.Collections.Generic;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Game.LibraryExtensions;

namespace Snowflake.Support.Remoting.GraphQl.Types.Model
{
    public class GameFileExtensionGraphType : ObjectGraphType<IGameFileExtension>
    {
        public GameFileExtensionGraphType()
        {
            Name = "GameFiles";
            Description = "The files of a game";

            Field<FileRecordGraphType>("files",
                description: "The files for which have metadata and are installed, not all files.",
                resolve: context => context.Source.Files);

            Field<ListGraphType<FileGraphType>>("programFiles",
               description: "The files for which have metadata and are installed ",
               arguments: new QueryArguments(new QueryArgument<BooleanGraphType> {  Name = "recursive", DefaultValue = false, }),
               resolve: context => context.GetArgument("recursive", false) ? context.Source.ProgramRoot.EnumerateFilesRecursive() 
                                : context.Source.ProgramRoot.EnumerateFiles());


        }
    }
}
