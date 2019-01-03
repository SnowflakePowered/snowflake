using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL.Types;
using Snowflake.Model.Game.LibraryExtensions;

namespace Snowflake.Support.Remoting.GraphQL.Types.Model
{
    public class GameConfigurationExtensionGraphType : ObjectGraphType<IGameConfigurationExtension>
    {
        public GameConfigurationExtensionGraphType()
        {
            Name = "GameConfigurations";
            Description = "The configurations of a game";

            Field<ListGraphType<StringGraphType>>("emulatorTypes",
                description: "Emulators for which this game has configs.",
                resolve: context => context.Source.GetProfileNames().Select(k => k.Key));
        }
    }
}
