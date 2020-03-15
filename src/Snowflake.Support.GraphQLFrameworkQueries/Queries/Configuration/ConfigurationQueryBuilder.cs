using HotChocolate;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Support.GraphQLFrameworkQueries.Queries.Configuration
{
    [GraphQLResolverOf("Game")]
    public class ConfigurationQueryBuilder
    {
        public string Test([Parent]IGame game, string input)
        {
            return game.Record.PlatformID + input;
        }
    }
}
