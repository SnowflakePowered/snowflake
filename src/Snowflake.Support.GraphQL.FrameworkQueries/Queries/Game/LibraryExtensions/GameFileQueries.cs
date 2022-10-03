 using HotChocolate.Types;
using Snowflake.Remoting.GraphQL.Model.Game;
using Snowflake.Model.Game;
using Snowflake.Model.Game.LibraryExtensions;
using Snowflake.Support.GraphQLFrameworkQueries.Queries.Game.LibraryExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using Snowflake.Remoting.GraphQL;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.LibraryExtensions
{
    public class GameFileQueries
        : ObjectTypeExtension<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.ExtendGame();

            descriptor.Field("files")
                .Type<GameFileExtensionType>()
                .Description("Provides access to the game's files. If this game does not have a registered content library, returns null.")
                .Resolve(context =>
                {
                    try
                    {
                        return context.Parent<IGame>().WithFiles();
                    }
                    catch
                    {
                        return null;
                    }
                });
        }
    }
}
