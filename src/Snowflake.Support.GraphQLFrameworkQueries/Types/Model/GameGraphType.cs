using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Model.Records.Game;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
{
    public class GameGraphType : ObjectGraphType<IGame>
    {
        public GameGraphType()
        {
            Name = "Game";
            Description = "An executable game";

            //Field<ListGraphType<FileRecordGraphType>>(
            //    "files",
            //    description: "A list of files associated with this game.",
            //    resolve: context => context.Source.WithFiles().FileRecords);

            Field<GameRecordGraphType>("record",
                description: "The record information of this game.",
                resolve: context => context.Source.Record);

            Field<GameFileExtensionGraphType>("files",
                description: "The files of this game.",
                resolve: context => context.Source.WithFiles());

            Field<GameConfigurationExtensionGraphType>("configs",
                description: "The files of this game.",
                resolve: context => context.Source.WithConfigurations());
        }
    }
}
