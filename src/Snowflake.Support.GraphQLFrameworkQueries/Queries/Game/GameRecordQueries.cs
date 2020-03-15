using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Game;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Game
{
    public class GameRecordQueries
        : ObjectTypeExtension<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.Name("Game");

            descriptor.Field(g => g.Record)
                .Name("record")
                .Type<GameRecordType>()
                .Description("Record metadata relating to this game.")
                ;

            //descriptor.Field<IGame>(g => g.WithConfigurations())
            //    .Name("configs")
            //    .Resolver(g => g.Parent<IGame>)
        }
    }
}
