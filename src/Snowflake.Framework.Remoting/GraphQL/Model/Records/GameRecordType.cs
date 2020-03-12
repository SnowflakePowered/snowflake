using HotChocolate.Types;
using Snowflake.Model.Records.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Game
{
    internal sealed class GameRecordType
        : ObjectType<IGameRecord>
    {
        protected override void Configure(IObjectTypeDescriptor<IGameRecord> descriptor)
        {
            descriptor.Name("GameRecord")
                .Description("The record associated with a Game and its associated metadata.");

            descriptor.Field(g => g.Title)
                .Description("The title of the game.");

            descriptor.Field(g => g.PlatformID)
                .Description("The original platform or game console of the game this object represents.");

            descriptor.Field(g => g.RecordID)
                .Name("guid")
                .Description("The unique ID of the game.");

            descriptor.Field(g => g.Metadata.Select(m => m.Value))
                .Name("metadata")
                .Description("The metadata associated with this game.")
                .UseFiltering();
        }
    }
}
