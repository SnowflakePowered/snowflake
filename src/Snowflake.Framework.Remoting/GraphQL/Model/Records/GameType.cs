using HotChocolate.Types;
using Snowflake.Framework.Remoting.GraphQL.Model.Game;
using Snowflake.Model.Game;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snowflake.Framework.Remoting.GraphQL.Model.Records
{
    public class GameType
        :  ObjectType<IGame>
    {
        protected override void Configure(IObjectTypeDescriptor<IGame> descriptor)
        {
            descriptor.Name("Game")
                .Description("Represents all information regarding a particular game.")
                .BindFieldsExplicitly();
            descriptor.Field(g => g.Record)
                .Type<GameRecordType>()
                .Description("Record metadata relating to this game.");
        }
    }
}
